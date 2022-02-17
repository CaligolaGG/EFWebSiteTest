<template> 
  <div class="container">
  <Modal @deleteItem="deleteProduct"></Modal>

    <div class="row">
      <div class="col-10">
        <h2 class="bold">Products</h2></div>
      <div class="col-2 ">
        <button class="btn btn-outline-primary float-end" @click="$router.push({path:'/products/new'})" > AddProduct</button> <br>
      </div> <hr>
    </div>
      <div class="alert alert-danger" role="alert" v-bind:class="{'d-none':!alertActive}"  >
      <button type="button" class="btn-close float-end"  @click="removeAlert()"  aria-label="Close"></button>
        No Products Found
      </div>
    <div v-if="this.loading" >
      <Skeleton></Skeleton>
    </div>
    <div v-if="!this.loading" >
      <table class="table table-striped table-light ">
        <thead > 
          <tr>
            <th scope="col" class="position-relative hoverV2"  @click="selectOrderBy(Order.Brand)">Brand  
              <i  class="bi bi-caret-down-fill position-absolute bottom-0 end-0 sortArrow " v-bind:class="{'text-primary':selectArrow(0,false)}"> </i>
              <i class="bi bi-caret-up-fill position-absolute top-0 end-0 sortArrow" v-bind:class="{'text-primary':selectArrow(0,true)}" ></i>
              <i class="bi bi-caret-down-fill  position-absolute bottom-0 end-0 text-primary sortArrow" v-if="defaultArrowState" ></i>
            </th>
              
            <th scope="col" class="position-relative hoverV2" @click="selectOrderBy(Order.Name)">Product 
              <i class="bi bi-caret-down-fill  position-absolute bottom-0 end-0 sortArrow " v-bind:class="{'text-primary':selectArrow(1,false)}"></i>
              <i class="bi bi-caret-up-fill  position-absolute top-0 end-0 sortArrow" v-bind:class="{'text-primary':selectArrow(1,true)}" ></i>
              <i class="bi bi-caret-down-fill  position-absolute bottom-0 end-0 text-primary sortArrow" v-if="defaultArrowState" ></i>
            </th>
            <th scope="col" class="position-relative" >Categories </th>
            <th scope="col" class="position-relative hoverV2" @click="selectOrderBy(Order.Price)">Price
              <i class="bi bi-caret-down-fill  position-absolute bottom-0 end-0 sortArrow" v-bind:class="{'text-primary':selectArrow(2,false)}"></i> 
              <i class="bi bi-caret-up-fill position-absolute top-0 end-0 sortArrow" v-bind:class="{'text-primary':selectArrow(2,true)}"></i>
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
        
        <tbody v-if="!alertActive">
          <tr v-for="item in this.getProducts" :key="item.Id" class="hover" @click="$router.push({path:'/products/'+item.id})">
            <td class="col-2" >{{item.brandName}}</td>
            <td class="col-4" > <b> {{item.productName}} </b> |  {{item.description}}</td>
            <td class="col-3" ><span v-for="(cat, index) in item.categories" :key="index" class="rounded-pill bg-primary text-light m-1 ">
              <small class="p-1" v-bind:title="cat" > {{cat.substring(0, 10)}} </small> </span> 
            </td>
            <td class="col-1" > $ {{item.price}} </td>
            <td class="col-1">
              <div class="input-group ">
                <button class="  btn btn-outline-secondary bi bi-pencil-square" @click.stop="$router.push({path:'/products/'+item.id+'/edit'})"></button>
                <button class=" btn btn-outline-secondary text-danger bi bi-trash-fill " data-bs-toggle="modal" data-bs-target="#exampleModal" @click.stop="deleteItem = item.id"></button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>     
      <Paging v-if="!alertActive" @changePage="fetchPage" v-bind:totalPagesNumber="info.data.totalPagesNumber"/> 
    </div>

    
  </div>
</template>


<script>
import Repository from "../../Api/RepoFactory";
import Paging from "../Pagination.vue";
import Skeleton from "../Skeleton.vue";
import Modal from "../Modal.vue"
const ProductsRepository = Repository.get("products");
const BrandRepository = Repository.get("brands");


export default {
  data(){
   return {
     loading: true,             //id of the product (from routing)
     lastcurrentpage:1,         //used to keep the current page when deleting an item

     orderBy:4 ,                 //integer to choose the criteria of ordering
     isAsc:true,                //boolean to indicate if the ordering is ascendent or discendent
     brandChosen:0,             //search through the ID of a brand
     defaultArrowState:true,    //indicate if the default ordering is active

     info:{},                   //object to contain the list of products fetched from the db
     
     alertActive:false,          //indicate if a problem raised and the alert has to be shown on screen
     Order :Object.freeze({
        Brand: 0,
        Name:1,
        Price:2,
        Default:3,
      }),
      deleteItem:null, //id of item to delete
    
   }
  }, 
  components:{
     Paging,
     Skeleton,
     Modal
  },

  methods:{
    //fetch a page of products through the repository get method
    async fetchPage(pageNum=1){
      var error = false
      this.lastcurrentpage = pageNum
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
    async deleteProduct(){
        await ProductsRepository.delete(this.deleteItem);
        this.fetchPage(this.lastcurrentpage);
    },
    //calculates which arrow is active. Returns a boolean
    selectArrow(orderBy, isAsc ){
      return this.orderBy == orderBy && this.isAsc == isAsc
    },
    removeAlert(){
      this.alertActive = false;
      this.brandChosen =0;
      this.fetchPage();
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

