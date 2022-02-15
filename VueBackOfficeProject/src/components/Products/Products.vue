<template> 
  <div class="container">
    <div class="row">
      <div class="col-10">
        <h2 class="bold">Products</h2></div>
      <div class="col ">
        <button class="btn btn-outline-primary   " @click="$router.push({path:'/products/new'})" > AddProduct</button> <br>
      </div> <hr>
    </div>
      <div class="alert alert-danger" role="alert" v-bind:class="{'d-none':!alertActive}"  >
        No Products Found
      </div>
    <div v-if="this.loading" >
      <Skeleton></Skeleton>
    </div>
    <div v-if="!this.loading" >
      <table class="table table-striped table-light ">
        <thead > 
          <tr>
            <th scope="col" class="position-relative hoverV2"  @click="selectOrderBy(1)">Brand  
              <i  class="bi bi-caret-down-fill position-absolute bottom-0 end-0 sortArrow " v-bind:class="{'text-primary':selectArrow(1,false)}"> </i>
              <i class="bi bi-caret-up-fill position-absolute top-0 end-0 sortArrow" v-bind:class="{'text-primary':selectArrow(1,true)}" ></i>
              <i class="bi bi-caret-down-fill  position-absolute bottom-0 end-0 text-primary sortArrow" v-if="defaultArrowState" ></i>
            </th>
              
            <th scope="col" class="position-relative hoverV2" @click="selectOrderBy(2)">Product 
              <i class="bi bi-caret-down-fill  position-absolute bottom-0 end-0 sortArrow " v-bind:class="{'text-primary':selectArrow(2,false)}"></i>
              <i class="bi bi-caret-up-fill  position-absolute top-0 end-0 sortArrow" v-bind:class="{'text-primary':selectArrow(2,true)}" ></i>
              <i class="bi bi-caret-down-fill  position-absolute bottom-0 end-0 text-primary sortArrow" v-if="defaultArrowState" ></i>
            </th>
            <th scope="col" class="position-relative" >Categories </th>
            <th scope="col" class="position-relative hoverV2" @click="selectOrderBy(3)">Price
              <i class="bi bi-caret-down-fill  position-absolute bottom-0 end-0 sortArrow" v-bind:class="{'text-primary':selectArrow(3,false)}"></i> 
              <i class="bi bi-caret-up-fill position-absolute top-0 end-0 sortArrow" v-bind:class="{'text-primary':selectArrow(3,true)}"></i>
            </th>
            <th scope="col">  </th>
          </tr>
          <tr class="bg-light">
            <td> 
              <select name="" id="" class="form-select m-1 "  v-model="brandChosen" @change="fetchPage()">
                <option default value="0">  No Brand </option>
                <option v-for="brand in this.brands" :key="brand.Id" v-bind:value="brand.id" > {{brand.name}} </option>  
              </select>  
            </td>
            <td colspan="4"></td>
          </tr>
        </thead>
        
        <tbody >
          <tr v-for="item in this.getProducts" :key="item.Id" class="hover" @click="$router.push({path:'/products/'+item.id})">
            <td class="col-2" >{{item.brandName}}</td>
            <td class="col-3" > <b> {{item.productName}} </b> |  {{item.description}}</td>
            <td class="col-3" ><span v-for="(cat, index) in item.categories" :key="index" class="rounded-pill bg-primary text-light ">  <small class="p-1"> {{cat}} </small> </span> </td>
            <td class="col-2" >{{item.price}}</td>
            <td class="col-1">
              <div class="input-group">
                <button class="col offset-1 btn btn-outline-secondary bi bi-pencil-square" @click.stop="$router.push({path:'/products/'+item.id+'/edit'})"></button>
                <button class="col btn btn-outline-secondary text-danger bi bi-trash-fill " @click.stop="deleteProduct(item.id)"></button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>     
      <Paging @changePage="fetchPage" v-bind:totalPagesNumber="info.data.totalPagesNumber"/> 
    </div>

    
  </div>
</template>


<script>
import Repository from "../../Api/RepoFactory";
import Paging from "../Pagination.vue";
import Skeleton from "../Skeleton.vue";
const ProductsRepository = Repository.get("products");
const BrandRepository = Repository.get("brands");

export default {
  data(){
   return {
     loading: true,             //id of the product (from routing)

     orderBy:0,                 //integer to choose the criteria of ordering
     isAsc:true,                //boolean to indicate if the ordering is ascendent or discendent
     brandChosen:0,             //search through the ID of a brand
     defaultArrowState:true,    //indicate if the default ordering is active

     info:{},                   //object to contain the list of products fetched from the db
     
     alertActive:false          //indicate if a problem raised and the alert has to be shown on screen

   }
  }, 
  components:{
     Paging,
     Skeleton
  },

  methods:{
    //fetch a page of products through the repository get method
    async fetchPage(pageNum=1){
      var error = false
      let temp=await ProductsRepository.get(pageNum, this.orderBy,this.isAsc,this.brandChosen)
                                          .catch(()=>{this.alertActive = true; error=true});
      if(!error)
      {
        this.info = temp
        this.alertActive = false
      }
    },
    //used to load all initial needed data. In particular the first page of products, all brands and all categories
    async getData(){ 
      await this.fetchPage();
      let temp = await BrandRepository.get();
      this.brands = temp.data
      this.loading = false;
    },
    //changes the ordering of the data
    async selectOrderBy(n){
      this.defaultArrowState = false
      this.orderBy==n? this.isAsc = !this.isAsc : this.orderBy= n;
      this.fetchPage();
    },
    //delete a product and refreshes the page
    async deleteProduct(id){
      if(confirm("are you sure you want to delete this product?"))
      {
        await ProductsRepository.delete(id);
        this.fetchPage();
      }
    },
    //calculates which arrow is active. Returns a boolean
    selectArrow(orderBy, isAsc ){
      return this.orderBy == orderBy && this.isAsc == isAsc
    }

  },
  computed:{
    //returns the list of products from the info fetched.
    getProducts(){
      return this.info.data.listEntities
    },
  },
  async created(){
    await this.getData();
  }
  
}
</script>

