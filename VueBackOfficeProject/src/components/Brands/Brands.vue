<template>
  <div class="container" >
    <div class="row">
      <div class="col-10">
        <h2> Brands </h2>
      </div> 
      <div class="col">
        <button class="btn btn-outline-primary float-end" @click="$router.push({path:'/brands/new'})">AddBrand</button>
      </div>
    </div><hr>
    <div class="alert alert-danger" role="alert" v-bind:class="{'d-none':!alertActive}" >
      <button type="button" class="btn-close float-end"  @click="removeAlert()"  aria-label="Close"></button>
      No Brands Found
    </div>

    <Modal @deleteItem="Remove"></Modal>


    <div v-if="this.loading" >
      <Skeleton></Skeleton>
    </div>
    
    <div  v-if="!this.loading">
        <table class="table table-striped table-light">
            <thead >
              <tr>
                  <th scope="col" class="position-relative" >BrandId </th>
                  <th scope="col" class="position-relative" >BrandName </th>
                  <th scope="col" class="position-relative" >Description </th>
                  <th scope="col"></th>
              </tr>
              <tr class="bg-light">
                <td></td>
                  <td>
                    <div class="row">
                      <div class="col">
                        <input @keyup.enter="fetchPage()" v-debounce:300ms="searchDebounced" class="form-control" type="text" v-model="search" placeholder="BrandName">
                      </div>
                    </div>
                  </td> <td colspan=2></td> 
              </tr>
            </thead>

            <tbody v-if="!alertActive">
                <tr class="bg-light hover" v-for="brand in this.getBrands" :key="brand.id" @click.stop="$router.push({path:'/brands/'+brand.id})">
                    <td class="col-2" >{{brand.id}}</td>
                    <td class="col-4">{{brand.brandName}}</td>
                    <td class="col-4">{{brand.description}}</td>
                    <td class="col-1">
                      <div class="input-group">
                          <button class="btn btn-outline-secondary bi bi-pencil-square" @click.stop="$router.push({path:'/brands/'+brand.id+'/edit'})"> </button>
                          <button class="btn btn-outline-secondary text-danger bi bi-trash-fill " data-bs-toggle="modal" data-bs-target="#exampleModal" @click.stop="selectItem(brand)">  </button>
                      </div>
                    </td>
                </tr>
            </tbody>
        </table>
        <Paging v-if="!alertActive"  @changePage="fetchPage" v-bind:totalPagesNumber="info.data.totalPagesNumber"/> 
    </div>
    <ToastComponent v-show="toastActive" v-bind:itemType="'Brand'" v-bind:deleteItemName="deleteItemName" ></ToastComponent>
  </div>
</template>


<script lang="js">
import Vue from 'vue'
import Repository from "../../Api/RepoFactory";
import vueDebounce from 'vue-debounce';
import Paging from "../Pagination.vue";
import Skeleton from "../Skeleton.vue";
import Modal from "../Modal.vue";
import {Toast} from "bootstrap/dist/js/bootstrap.esm.js";
import ToastComponent from "../Toast.vue"

const BrandRepository = Repository.get("brands");

Vue.use(vueDebounce)


export default {
  data(){
   return {
    loading: true,     //id of the product (from routing)
    search:"",         //variable that holds the  string searched by the user
    lastcurrentpage:1,         //used to keep the current page when deleting an item


    info:{},           //object to contain the list of products fetched from the db
    alertActive:false,  //indicate if a problem raised and the alert has to be shown on screen
    deleteItem:null,
    deleteItemName:null,
    toastActive:false,
   }
  },
  components:{
    Skeleton,
    Paging,
    Modal,
    ToastComponent,
  },

  methods:{
    //fetch a page of products through the repository get method
    async fetchPage(pageNum=1){
        var error = false
        this.lastcurrentpage = pageNum

        let temp =  await BrandRepository.getAll(pageNum,this.search.trim(),10)
                                        .catch(()=>{this.alertActive=true; error=true});
        if(!error)
        {
          this.alertActive = false
          this.info = temp
        }
    },
    searchDebounced(){if (this.search.length > 3 || this.search.length ==0) this.fetchPage()},
    //used to load all initial needed data. In particular the first page of products, all brands and all categories
    async getData(){ 
      await this.fetchPage();
      this.loading = false;
    },
    //remove the brand selected
    async Remove() {
      await BrandRepository.delete(this.deleteItem);
      this.toastActive = true
      var toast = new Toast(document.getElementById("liveToast"));
      toast.show();
      this.fetchPage(this.lastcurrentpage);
    },
    removeAlert(){
      this.alertActive = false;
      this.search = "";
      this.fetchPage();

    },
    selectItem(brand){
      this.deleteItem = brand.id
      this.deleteItemName = brand.brandName
    },


  },
  computed:{
    //returns the list of products from the info fetched.
    getBrands(){
      return this.info.data.listEntities
    },

  },
  async created(){
    await this.getData();

  },
  
}
</script>