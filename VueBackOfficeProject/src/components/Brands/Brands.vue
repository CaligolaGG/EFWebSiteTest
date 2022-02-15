<template>
  <div class="container" >
    <div class="row">
      <div class="col-10">
        <h2> Brands </h2>
      </div> 
      <div class="col">
        <button class="btn btn-outline-primary mx-2" @click="$router.push({path:'/brands/new'})">AddBrand</button>
      </div>
    </div><hr>
        <div class="alert alert-danger" role="alert" v-bind:class="{'d-none':!alertActive}" >
          No Brands Found
        </div>
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
                          <input v-debounce:300ms="searchDebounced" class="form-control" type="text" v-model="search" placeholder="BrandName">
                        </div>
                      </div>
                    </td><td></td> <td></td>
                </tr>
              </thead>

              <tbody >
                  <tr class="bg-light hover" v-for="brand in this.getBrands" :key="brand.brandId" @click.stop="$router.push({path:'/brands/'+brand.brandId})">
                      <td class="col-2" >{{brand.brandId}}</td>
                      <td class="col-4">{{brand.brandName}}</td>
                      <td class="col-4">{{brand.description}}</td>
                      <td class="col-1">
                        <div class="input-group">
                            <button class="col offset-1 btn btn-outline-secondary bi bi-pencil-square" @click.stop="$router.push({path:'/brands/'+brand.brandId+'/edit'})"> </button>
                            <button class="col btn btn-outline-secondary text-danger bi bi-trash-fill " @click.stop="Remove(brand.brandId)">  </button>
                        </div>
                      </td>
                  </tr>
              </tbody>
          </table>
          <Paging @changePage="fetchPage" v-bind:totalPagesNumber="info.data.totalPagesNumber"/> 
      </div>
  </div>
</template>


<script lang="js">
import Vue from 'vue'
import Repository from "../../Api/RepoFactory";
import vueDebounce from 'vue-debounce';
import Paging from "../Pagination.vue";
import Skeleton from "../Skeleton.vue";

const BrandRepository = Repository.get("brands");

Vue.use(vueDebounce)


export default {
  data(){
   return {
    loading: true,     //id of the product (from routing)
    search:"",         //variable that holds the  string searched by the user

    info:{},           //object to contain the list of products fetched from the db
    alertActive:false  //indicate if a problem raised and the alert has to be shown on screen

   }
  },
  components:{
    Skeleton,
    Paging,
  },

  methods:{
    //fetch a page of products through the repository get method
    async fetchPage(pageNum=1){
        var error = false

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
    async Remove(id) {
        if(confirm("are you sure you want to delete this brand?"))
        {
            await BrandRepository.delete(id);
            this.fetchPage();
        }
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