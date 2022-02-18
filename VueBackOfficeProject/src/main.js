import Vue from 'vue'
import App from './App.vue'
import router from './Router'
import 'bootstrap/dist/js/bootstrap.js'


Vue.config.productionTip = false

new Vue({
  render: h => h(App),
  router,
}).$mount('#app')
