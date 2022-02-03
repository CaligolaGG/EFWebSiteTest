import Vue from 'vue'
import App from './App.vue'
import router from './Router'
import bootstrapvue from 'bootstrap-vue'
import 'bootstrap-vue/dist/bootstrap-vue.css'

Vue.config.productionTip = false
Vue.use(bootstrapvue)

new Vue({
  render: h => h(App),
  router,
}).$mount('#app')
