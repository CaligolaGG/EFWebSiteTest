import Vue from 'vue'
import Router from 'vue-router'

import Products from '@/components/Products.vue'
import Brands from '@/components/Brands.vue'
import Requests from '@/components/Requests.vue'


Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/products',
      name : 'products',
      component: Products
    },
    {
      path: '/leads',
      name : 'requests',
      component: Requests
    },
    {
      path: '/brands',
      name : 'brands',
      component: Brands
    },


  ]
})