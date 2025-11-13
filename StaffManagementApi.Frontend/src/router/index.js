import { createRouter, createWebHistory } from 'vue-router'
import HomeView from "../views/HomeView.vue";
import CreateStaff from '@/components/staff/Create.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [{
    path: "/",
    name: "home",
    component: HomeView,
  }, {
    path: "/new-staff",
    name: "new-staff",
    component: CreateStaff,
  }],
})

export default router
