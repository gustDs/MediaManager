import { createRouter, createWebHistory } from 'vue-router'
import MediaItemList from '../views/MediaItemList.vue'
import MediaItemDetail from '../views/MediaItemDetail.vue'

const routes = [
  { path: '/', component: MediaItemList },
  { path: '/media/:id', component: MediaItemDetail },
]

export default createRouter({
  history: createWebHistory(),
  routes,
})
