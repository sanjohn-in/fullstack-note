import { createApp } from 'vue'
import App from './App.vue'

// Styles first
import 'element-plus/dist/index.css'
import './style.css'

// Plugins
import pinia from '@/plugins/pinia'
import router from '@/routes/index'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'
import ElementPlus from 'element-plus'

const app = createApp(App)

// Register icons
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
}

// Register plugins
app.use(ElementPlus)
app.use(pinia)
app.use(router)

app.mount('#app')