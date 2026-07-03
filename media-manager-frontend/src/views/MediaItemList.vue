<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import Select from 'primevue/select'
import Button from 'primevue/button'
import { IconMovie, IconDeviceTv, IconDeviceGamepad2, IconBook } from '@tabler/icons-vue'
import * as mediaItemService from '../services/mediaItemService'
import * as consumptionRecordService from '../services/consumptionRecordService'

const router = useRouter()

const TYPE_CONFIG = {
  Filme: { color: '#7F77DD', icon: IconMovie,          label: 'Filme' },
  Serie: { color: '#378ADD', icon: IconDeviceTv,        label: 'Série' },
  Jogo:  { color: '#639922', icon: IconDeviceGamepad2,  label: 'Jogo'  },
  Livro: { color: '#BA7517', icon: IconBook,            label: 'Livro' },
}

const NAV_ITEMS = [
  { key: 'Todos', label: 'Todos', color: '#94a3b8' },
  { key: 'Filme', label: 'Filme', color: '#7F77DD' },
  { key: 'Serie', label: 'Série', color: '#378ADD' },
  { key: 'Jogo',  label: 'Jogo',  color: '#639922' },
  { key: 'Livro', label: 'Livro', color: '#BA7517' },
]

const tipoOptions = [
  { label: 'Filme', value: 'Filme' },
  { label: 'Série', value: 'Serie' },
  { label: 'Jogo',  value: 'Jogo'  },
  { label: 'Livro', value: 'Livro' },
]

const items = ref([])
const selectedType = ref('Todos')
const dialogVisible = ref(false)
const editingItem = ref(null)
const form = ref({ nome: '', tipo: null })

const activeColor = computed(() =>
  selectedType.value === 'Todos' ? '#94a3b8' : (TYPE_CONFIG[selectedType.value]?.color ?? '#94a3b8')
)
const activeLabel = computed(() =>
  selectedType.value === 'Todos' ? 'Item' : (TYPE_CONFIG[selectedType.value]?.label ?? 'Item')
)
const filteredItems = computed(() =>
  selectedType.value === 'Todos' ? items.value : items.value.filter(i => i.tipo === selectedType.value)
)

function formatDate(dateStr) {
  if (!dateStr) return null
  return new Date(dateStr).toLocaleDateString('pt-BR')
}

async function loadItems() {
  const all = await mediaItemService.getAll()
  const sessionLists = await Promise.all(
    all.map(item => consumptionRecordService.getAllByMediaItem(item.id))
  )
  items.value = all.map((item, i) => ({
    ...item,
    sessionCount: sessionLists[i].length,
    lastSessionDate: sessionLists[i][0]?.criadoEm ?? null,
  }))
}

function openNew() {
  editingItem.value = null
  form.value = { nome: '', tipo: selectedType.value !== 'Todos' ? selectedType.value : null }
  dialogVisible.value = true
}

function openEdit(item) {
  editingItem.value = item
  form.value = { nome: item.nome, tipo: item.tipo }
  dialogVisible.value = true
}

async function save() {
  if (editingItem.value) {
    await mediaItemService.update(editingItem.value.id, { nome: form.value.nome })
  } else {
    await mediaItemService.create({ nome: form.value.nome, tipo: form.value.tipo })
  }
  dialogVisible.value = false
  await loadItems()
}

async function deleteItem(id) {
  await mediaItemService.remove(id)
  await loadItems()
}

onMounted(loadItems)
</script>

<template>
  <div style="display: flex; height: 100vh; overflow: hidden;">

    <!-- Sidebar -->
    <aside style="width: 200px; flex-shrink: 0; background: var(--surface-1); border-right: 0.5px solid var(--border); display: flex; flex-direction: column; padding: 24px 0;">
      <div style="padding: 0 16px 20px; font-weight: 700; font-size: 15px;">Media Manager</div>
      <nav>
        <button
          v-for="nav in NAV_ITEMS"
          :key="nav.key"
          @click="selectedType = nav.key"
          :style="{
            display: 'flex', alignItems: 'center', gap: '10px',
            width: '100%', padding: '9px 16px', border: 'none',
            background: selectedType === nav.key ? 'var(--surface-0)' : 'transparent',
            borderLeft: selectedType === nav.key ? `2px solid ${nav.color}` : '2px solid transparent',
            color: selectedType === nav.key ? '#e2e8f0' : '#94a3b8',
            cursor: 'pointer', fontSize: '14px', textAlign: 'left',
          }"
        >
          <span :style="{ width: '8px', height: '8px', borderRadius: '50%', background: nav.color, flexShrink: 0 }"></span>
          {{ nav.label }}
        </button>
      </nav>
    </aside>

    <!-- Main -->
    <div style="flex: 1; overflow-y: auto; display: flex; flex-direction: column;">

      <!-- Topbar -->
      <div style="display: flex; align-items: center; gap: 12px; padding: 18px 28px; border-bottom: 0.5px solid var(--border); background: var(--surface-1);">
        <span style="font-size: 18px; font-weight: 700;">
          {{ selectedType === 'Todos' ? 'Todos' : TYPE_CONFIG[selectedType]?.label }}
        </span>
        <span style="font-size: 12px; background: var(--surface-0); border: 0.5px solid var(--border); border-radius: 20px; padding: 2px 10px; color: #64748b;">
          {{ filteredItems.length }}
        </span>
        <div style="margin-left: auto;">
          <Button
            :label="`Nova ${activeLabel}`"
            icon="pi pi-plus"
            :style="{ background: activeColor, borderColor: activeColor }"
            @click="openNew"
          />
        </div>
      </div>

      <!-- Cards -->
      <div style="padding: 20px 28px; display: flex; flex-direction: column; gap: 8px;">
        <div
          v-for="item in filteredItems"
          :key="item.id"
          style="display: flex; align-items: center; gap: 14px; background: var(--surface-2); border: 0.5px solid var(--border); border-radius: 12px; overflow: hidden;"
        >
          <!-- Color bar -->
          <div :style="{ width: '4px', alignSelf: 'stretch', background: TYPE_CONFIG[item.tipo]?.color, flexShrink: 0 }"></div>

          <!-- Type icon -->
          <div :style="{ color: TYPE_CONFIG[item.tipo]?.color, flexShrink: 0, paddingTop: '1px' }">
            <component :is="TYPE_CONFIG[item.tipo]?.icon" :size="20" />
          </div>

          <!-- Name + meta -->
          <div style="flex: 1; min-width: 0; padding: 14px 0;">
            <div class="item-name" @click="router.push(`/media/${item.id}`)">{{ item.nome }}</div>
            <div style="font-size: 12px; color: #64748b; margin-top: 2px;">
              {{ item.sessionCount }} {{ item.sessionCount === 1 ? 'sessão' : 'sessões' }}
              <template v-if="item.lastSessionDate"> · última em {{ formatDate(item.lastSessionDate) }}</template>
            </div>
          </div>

          <!-- Badge -->
          <div style="flex-shrink: 0;">
            <span v-if="item.statusAtual === 'Done'"       class="badge badge-done">Done</span>
            <span v-else-if="item.statusAtual === 'Doing'" class="badge badge-doing">Doing</span>
            <span v-else-if="item.statusAtual === 'Do'"    class="badge badge-do">Do</span>
          </div>

          <!-- Actions -->
          <div style="display: flex; gap: 2px; padding-right: 10px; flex-shrink: 0;">
            <Button icon="pi pi-pencil" text rounded size="small" @click.stop="openEdit(item)" />
            <Button icon="pi pi-trash"  text rounded size="small" severity="danger" @click.stop="deleteItem(item.id)" />
          </div>
        </div>

        <div v-if="filteredItems.length === 0" style="text-align: center; color: #94a3b8; padding: 56px 0; font-size: 14px;">
          Nenhum item encontrado.
        </div>
      </div>
    </div>
  </div>

  <!-- Dialog -->
  <Dialog v-model:visible="dialogVisible" :header="editingItem ? 'Editar Item' : 'Novo Item'" modal style="width: 400px">
    <div class="flex flex-col gap-4 pt-2">
      <div class="flex flex-col gap-1">
        <label>Nome</label>
        <InputText v-model="form.nome" />
      </div>
      <div class="flex flex-col gap-1">
        <label>Tipo</label>
        <Select v-model="form.tipo" :options="tipoOptions" optionLabel="label" optionValue="value"
                placeholder="Selecione o tipo" :disabled="!!editingItem" />
      </div>
    </div>
    <template #footer>
      <Button label="Cancelar" text @click="dialogVisible = false" />
      <Button label="Salvar" :style="{ background: activeColor, borderColor: activeColor }" @click="save" />
    </template>
  </Dialog>
</template>

<style scoped>
.item-name {
  font-weight: 600;
  font-size: 15px;
  cursor: pointer;
}
.item-name:hover { text-decoration: underline; }

.badge {
  font-size: 12px;
  font-weight: 500;
  border-radius: 20px;
  padding: 3px 10px;
}
.badge-done  { background: #EAF3DE; color: #3B6D11; }
.badge-doing { background: #FAEEDA; color: #854F0B; }
.badge-do    { background: var(--surface-0); color: #64748b; border: 0.5px solid var(--border); }
</style>
