<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import Dialog from 'primevue/dialog'
import ConfirmDialog from 'primevue/confirmdialog'
import Button from 'primevue/button'
import Select from 'primevue/select'
import InputNumber from 'primevue/inputnumber'
import Textarea from 'primevue/textarea'
import DatePicker from 'primevue/datepicker'
import { useConfirm } from 'primevue/useconfirm'
import * as mediaItemService from '../services/mediaItemService'
import * as consumptionRecordService from '../services/consumptionRecordService'
import { removeToken } from '../services/authService'

const route = useRoute()
const router = useRouter()
const confirm = useConfirm()

const NAV_ITEMS = [
  { key: 'Todos', label: 'Todos', color: '#94a3b8' },
  { key: 'Filme', label: 'Filme', color: '#7F77DD' },
  { key: 'Serie', label: 'Série', color: '#378ADD' },
  { key: 'Jogo',  label: 'Jogo',  color: '#639922' },
  { key: 'Livro', label: 'Livro', color: '#BA7517' },
]

const TYPE_COLOR = {
  Filme: '#7F77DD', Serie: '#378ADD', Jogo: '#639922', Livro: '#BA7517',
}

const mediaItem = ref(null)
const records = ref([])
const dialogVisible = ref(false)
const editingRecord = ref(null)
const saveError = ref(null)
const form = ref({
  status: null, dataInicio: null, dataFim: null,
  nota: null, resenha: null, horasJogadas: null, paginasLidas: null,
})

const statusOptions = [
  { label: 'Do', value: 'Do' },
  { label: 'Doing', value: 'Doing' },
  { label: 'Done', value: 'Done' },
]

const itemColor = computed(() => mediaItem.value ? (TYPE_COLOR[mediaItem.value.tipo] ?? '#94a3b8') : '#94a3b8')
const statusAtual = computed(() => records.value.length > 0 ? records.value[0].status : null)

function formatDate(dateStr) {
  if (!dateStr) return null
  return new Date(dateStr).toLocaleDateString('pt-BR')
}

function toISOOrNull(date) {
  return date ? date.toISOString() : null
}


async function load() {
  const id = route.params.id
  mediaItem.value = await mediaItemService.getById(id)
  records.value = await consumptionRecordService.getAllByMediaItem(id)
}

function openNew() {
  editingRecord.value = null
  form.value = { status: null, dataInicio: null, dataFim: null, nota: null, resenha: null, horasJogadas: null, paginasLidas: null }
  saveError.value = null
  dialogVisible.value = true
}

function openEdit(record) {
  editingRecord.value = record
  form.value = {
    status: record.status,
    dataInicio: record.dataInicio ? new Date(record.dataInicio) : null,
    dataFim: record.dataFim ? new Date(record.dataFim) : null,
    nota: record.nota,
    resenha: record.resenha,
    horasJogadas: record.horasJogadas,
    paginasLidas: record.paginasLidas,
  }
  saveError.value = null
  dialogVisible.value = true
}

async function save() {
  saveError.value = null
  try {
    const payload = {
      status: form.value.status,
      dataInicio: toISOOrNull(form.value.dataInicio),
      dataFim: toISOOrNull(form.value.dataFim),
      nota: form.value.nota,
      resenha: form.value.resenha,
      horasJogadas: form.value.horasJogadas,
      paginasLidas: form.value.paginasLidas,
    }
    if (editingRecord.value) {
      await consumptionRecordService.update(editingRecord.value.id, payload)
    } else {
      await consumptionRecordService.create({ ...payload, mediaItemId: route.params.id })
    }
    dialogVisible.value = false
    await load()
  } catch (e) {
    saveError.value = e.message
  }
}

function deleteRecord(id) {
  confirm.require({
    message: 'Tem certeza que deseja excluir esta sessão?',
    header: 'Confirmar exclusão',
    icon: 'pi pi-trash',
    rejectLabel: 'Cancelar',
    acceptLabel: 'Excluir',
    acceptClass: 'p-button-danger',
    accept: async () => {
      await consumptionRecordService.remove(id)
      await load()
    }
  })
}

onMounted(load)

function logout() {
  removeToken()
  router.push('/login')
}
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
          @click="router.push('/')"
          :style="{
            display: 'flex', alignItems: 'center', gap: '10px',
            width: '100%', padding: '9px 16px', border: 'none',
            background: mediaItem && nav.key === mediaItem.tipo ? 'var(--surface-0)' : 'transparent',
            borderLeft: mediaItem && nav.key === mediaItem.tipo ? `2px solid ${nav.color}` : '2px solid transparent',
            color: mediaItem && nav.key === mediaItem.tipo ? '#e2e8f0' : '#94a3b8',
            cursor: 'pointer', fontSize: '14px', textAlign: 'left',
          }"
        >
          <span :style="{ width: '8px', height: '8px', borderRadius: '50%', background: nav.color, flexShrink: 0 }"></span>
          {{ nav.label }}
        </button>
      </nav>
      <div style="margin-top: auto; padding: 0 8px 8px;">
        <button
          @click="logout"
          style="display: flex; align-items: center; gap: 10px; width: 100%; padding: 9px 8px; border: none; background: transparent; color: #94a3b8; cursor: pointer; font-size: 14px; border-radius: 6px;"
        >
          <span class="pi pi-sign-out" style="font-size: 14px;"></span>
          Sair
        </button>
      </div>
    </aside>

    <!-- Main -->
    <div v-if="mediaItem" style="flex: 1; overflow-y: auto; display: flex; flex-direction: column;">

      <!-- Topbar -->
      <div style="display: flex; align-items: center; gap: 14px; padding: 18px 28px; border-bottom: 0.5px solid var(--border); background: var(--surface-1);">
        <Button icon="pi pi-arrow-left" text rounded @click="router.push('/')" style="flex-shrink: 0;" />
        <div style="flex: 1; min-width: 0;">
          <div style="display: flex; align-items: center; gap: 10px; flex-wrap: wrap;">
            <span style="font-size: 18px; font-weight: 700;">{{ mediaItem.nome }}</span>
            <span v-if="statusAtual === 'Done'"       class="badge badge-done">Done</span>
            <span v-else-if="statusAtual === 'Doing'" class="badge badge-doing">Doing</span>
            <span v-else-if="statusAtual === 'Do'"    class="badge badge-do">Do</span>
          </div>
          <div style="font-size: 13px; color: #64748b; margin-top: 2px;">
            {{ mediaItem.tipo }} · {{ records.length }} {{ records.length === 1 ? 'sessão' : 'sessões' }}
          </div>
        </div>
        <Button
          label="Nova sessão"
          icon="pi pi-plus"
          :style="{ background: itemColor, borderColor: itemColor, flexShrink: 0 }"
          @click="openNew"
        />
      </div>

      <!-- Records -->
      <div style="padding: 20px 28px; display: flex; flex-direction: column; gap: 8px;">
        <div
          v-for="(record, index) in records"
          :key="record.id"
          style="display: flex; background: var(--surface-2); border: 0.5px solid var(--border); border-radius: 12px; overflow: hidden;"
        >
          <!-- Color bar -->
          <div :style="{ width: '4px', background: itemColor, flexShrink: 0 }"></div>

          <div style="flex: 1; padding: 14px 16px;">
            <!-- Row 1: session # + badge + actions -->
            <div style="display: flex; align-items: center; gap: 8px;">
              <span style="font-size: 13px; font-weight: 600; color: #64748b;">
                Sessão {{ records.length - index }}
              </span>
              <span v-if="record.status === 'Done'"       class="badge badge-done">Done</span>
              <span v-else-if="record.status === 'Doing'" class="badge badge-doing">Doing</span>
              <span v-else-if="record.status === 'Do'"    class="badge badge-do">Do</span>
              <div style="margin-left: auto; display: flex; gap: 2px;">
                <Button icon="pi pi-pencil" text rounded size="small" @click="openEdit(record)" />
                <Button icon="pi pi-trash"  text rounded size="small" severity="danger" @click="deleteRecord(record.id)" />
              </div>
            </div>

            <!-- Dates -->
            <div v-if="record.dataInicio || record.dataFim" style="font-size: 13px; color: #64748b; margin-top: 6px;">
              <template v-if="record.dataInicio">{{ formatDate(record.dataInicio) }}</template>
              <template v-if="record.dataInicio && record.dataFim"> → </template>
              <template v-if="record.dataFim">{{ formatDate(record.dataFim) }}</template>
            </div>

            <!-- Nota + extra -->
            <div style="display: flex; gap: 16px; margin-top: 6px; flex-wrap: wrap; align-items: center;">
              <span v-if="record.nota" style="display: inline-flex; align-items: center; gap: 1px;">
                <span v-for="i in 5" :key="i" style="position: relative; font-size: 15px; display: inline-block;">
                  <span style="color: #2d3748;">★</span>
                  <span
                    :style="{
                      position: 'absolute', left: 0, top: 0,
                      color: itemColor,
                      width: record.nota >= i ? '100%' : record.nota >= i - 0.5 ? '50%' : '0%',
                      overflow: 'hidden',
                      whiteSpace: 'nowrap'
                    }"
                  >★</span>
                </span>
                <span style="font-size: 12px; color: #94a3b8; margin-left: 4px;">{{ record.nota }}</span>
              </span>
              <span v-if="record.horasJogadas" style="font-size: 13px; color: #64748b;">{{ record.horasJogadas }}h jogadas</span>
              <span v-if="record.paginasLidas"  style="font-size: 13px; color: #64748b;">{{ record.paginasLidas }} págs. lidas</span>
            </div>

            <!-- Resenha -->
            <div v-if="record.resenha" style="font-size: 13px; font-style: italic; color: #64748b; margin-top: 6px; line-height: 1.5;">
              "{{ record.resenha }}"
            </div>
          </div>
        </div>

        <!-- Dashed add card -->
        <div class="dashed-card" @click="openNew">
          <span class="pi pi-plus" style="font-size: 13px;"></span>
          Nova sessão
        </div>
      </div>
    </div>
  </div>

  <!-- Dialog -->
  <Dialog v-model:visible="dialogVisible" :header="editingRecord ? 'Editar Sessão' : 'Nova Sessão'" modal style="width: 480px">
    <div class="flex flex-col gap-4 pt-2">
      <div class="flex flex-col gap-1">
        <label>Status</label>
        <Select v-model="form.status" :options="statusOptions" optionLabel="label" optionValue="value" placeholder="Selecione o status" />
      </div>
      <div class="flex flex-col gap-1">
        <label>Data Início</label>
        <DatePicker v-model="form.dataInicio" dateFormat="dd/mm/yy" showIcon />
      </div>
      <div class="flex flex-col gap-1">
        <label>Data Fim</label>
        <DatePicker v-model="form.dataFim" dateFormat="dd/mm/yy" showIcon />
      </div>
      <div class="flex flex-col gap-1">
        <label>Nota</label>
        <InputNumber v-model="form.nota" :min="0.5" :max="5.0" :step="0.5" :minFractionDigits="1" :maxFractionDigits="1" />
      </div>
      <div class="flex flex-col gap-1">
        <label>Resenha</label>
        <Textarea v-model="form.resenha" rows="3" />
      </div>
      <div v-if="mediaItem?.tipo === 'Jogo'" class="flex flex-col gap-1">
        <label>Horas Jogadas</label>
        <InputNumber v-model="form.horasJogadas" :min="0" />
      </div>
      <div v-if="mediaItem?.tipo === 'Livro'" class="flex flex-col gap-1">
        <label>Páginas Lidas</label>
        <InputNumber v-model="form.paginasLidas" :min="0" />
      </div>
    </div>
    <div v-if="saveError" style="color: #f87171; font-size: 13px; margin-top: 8px;">
      {{ saveError }}
    </div>
    <template #footer>
      <Button label="Cancelar" text @click="dialogVisible = false" />
      <Button label="Salvar" :style="{ background: itemColor, borderColor: itemColor }" @click="save" />
    </template>
  </Dialog>

  <ConfirmDialog />
</template>

<style scoped>
.badge {
  font-size: 12px;
  font-weight: 500;
  border-radius: 20px;
  padding: 3px 10px;
}
.badge-done  { background: #EAF3DE; color: #3B6D11; }
.badge-doing { background: #FAEEDA; color: #854F0B; }
.badge-do    { background: var(--surface-0); color: #64748b; border: 0.5px solid var(--border); }

.dashed-card {
  border: 1.5px dashed var(--border);
  border-radius: 12px;
  padding: 16px;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  color: #94a3b8;
  cursor: pointer;
  font-size: 14px;
  transition: opacity 0.15s;
}
.dashed-card:hover { opacity: 0.65; }
</style>
