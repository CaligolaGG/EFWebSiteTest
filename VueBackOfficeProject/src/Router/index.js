import Vue from 'vue'
import Router from 'vue-router'

import Products from '@/components/Products/Products.vue'
import ProductDetail from '@/components/Products/Detail.vue'
import ProductEdit from '@/components/Products/UpSert.vue'
import ProductAdd from '@/components/Products/UpSert.vue'

import Brands from '@/components/Brands/Brands.vue'
import BrandsDetail from '@/components/Brands/Detail.vue'
import BrandsEdit from '@/components/Brands/Edit.vue'
import BrandsAdd from '@/components/Brands/Add.vue'



import Requests from '@/components/Leads/Requests.vue'
import RequestsDetail from '@/components/Leads/Detail.vue'

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
      name : 'leads',
      component: Requests
    },
    {
      path: '/leads/:id',
      name : 'leadsDetail',
      component: RequestsDetail
    },

    {
      path: '/brands',
      name : 'brands',
      component: Brands
    },
    {
      path: '/brands/new',
      name : 'brandsNew',
      component: BrandsAdd
    },
    {
      path: '/brands/:id',
      name : 'brandsDetail',
      component: BrandsDetail
    },
    {
      path: '/brands/:id/edit',
      name : 'brandsEdit',
      component: BrandsEdit
    },



  ]
})