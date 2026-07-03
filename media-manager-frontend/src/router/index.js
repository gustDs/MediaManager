import { createRouter, createWebHistory } from 'vue-router'
import MediaItemList from '../views/MediaItemList.vue'
import MediaItemDetail from '../views/MediaItemDetail.vue'
import Login from '../views/Login.vue'
import { isAuthenticated } from '../services/authService'

const routes = [
  { path: '/login', component: Login },
  { path: '/', component: MediaItemList },
  { path: '/media/:id', component: MediaItemDetail },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

router.beforeEach((to) => {
  if (!isAuthenticated() && to.path !== '/login') return '/login'
})

export default router
