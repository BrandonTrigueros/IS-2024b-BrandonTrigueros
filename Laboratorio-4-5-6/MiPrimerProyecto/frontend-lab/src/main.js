import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from 'vue-router'
import ListaPaises from './components/ListaPaises.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [{ path: '/', name: "Home", component: ListaPaises }]
})

createApp(App).use(router).mount('#app')
