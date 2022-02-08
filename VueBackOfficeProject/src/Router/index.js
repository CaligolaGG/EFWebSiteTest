import Vue from 'vue'
import Router from 'vue-router'

import Products from '@/components/Products.vue'
import ProductDetail from '@/components/Products/Detail.vue'
import ProductEdit from '@/components/Products/Edit.vue'
import ProductAdd from '@/components/Products/Add.vue'

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
      path: '/products/new',
      name : 'productAdd',
      component: ProductAdd
    },
    {
      path: '/products/:id',
      name : 'productDetail',
      component: ProductDetail
    },
    {
      path: '/products/:id/edit',
      name : 'productEdit',
      component: ProductEdit
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