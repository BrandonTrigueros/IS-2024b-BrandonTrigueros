import { createApp } from 'vue'
import App from './App.vue'
import { createRouter, createWebHistory } from 'vue-router'
import ListaPaises from './components/ListaPaises.vue'
import PaisForm from './components/PaisForm.vue'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        { path: '/', name: "Home", component: ListaPaises },
        { path: '/pais', name: "Pais", component: PaisForm },
    ]
})

createApp(App).use(router).mount('#app')
