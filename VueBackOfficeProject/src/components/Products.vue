<template>
  <div class="mx-2">
    <button @click="$router.push({path:'/products/new'})" class="btn btn-primary"> AddProduct</button> <br>

    Products
    <div v-if="!this.loading" >
      <table class="table table-striped table-light ">
        <thead >
          <tr>
            <th scope="col" class="position-relative" @click="selectOrderBy(1)">Brand  
              <i  class="bi bi-caret-down-fill text-primary position-absolute bottom-0 end-0 sortArrow"> </i>
              <i class="bi bi-caret-up-fill text-primary position-absolute top-0 end-0 sortArrow" ></i></th>
            <th scope="col" class="position-relative" @click="selectOrderBy(2)">Product <i class="bi bi-caret-down-fill text-primary position-absolute bottom-0 end-0 sortArrow"></i> <i class="bi bi-caret-up-fill text-primary position-absolute top-0 end-0 sortArrow" ></i></th>
            <th scope="col" class="position-relative" >Categories </th>
            <th scope="col" class="position-relative" @click="selectOrderBy(3)">Price<i class="bi bi-caret-down-fill text-primary position-absolute bottom-0 end-0 sortArrow"></i> <i class="bi bi-caret-up-fill text-primary position-absolute top-0 end-0 sortArrow" ></i></th>
            <th scope="col">  </th>
          </tr>
        </thead>
        <tr class="bg-light">
          <td> 
            <select name="" id="" class="form-select m-1 "  v-model="brandName" @change="updateData()">
              <option value="">Select a brand</option>
              <option v-for="brand in this.brands" :key="brand.Id"  > {{brand.name}} </option>  
            </select>  
          </td>
          <td></td><td></td><td></td><td></td>
        </tr>
        <tbody >
          <tr v-for="item in this.getProducts" :key="item.Id" class="hover" >
            <td class="col" @click="$router.push({path:'/products/'+item.id})">{{item.brandName}}</td>
            <td class="col" @click="$router.push({path:'/products/'+item.id})"> <b> {{item.productName}} </b> |  {{item.description}}</td>
            <td class="col-4" @click="$router.push({path:'/products/'+item.id})"><span v-for="(cat, index) in item.categories" :key="index" class="rounded-pill bg-primary text-light ">  <small class="p-1"> {{cat}} </small> </span> </td>
            <td class="col" @click="$router.push({path:'/products/'+item.id})">{{item.price}}</td>
            <td class="col-2">
              <div class="mx-3 d-flex justify-content-end">
              <button class="col-3 offset-1 btn btn-outline-secondary bi bi-pencil-square" @click="$router.push({path:'/products/'+item.id+'/edit'})"></button>
              <button class="col-3 btn btn-outline-secondary text-danger" @click="deleteProduct(item.id)"><i class="bi bi-trash-fill text"></i></button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>      
      <ul class="pagination justify-content-center">
        <button @click="previousPage()" class="btn btn-primary mx-1">Previous</button>
        <button v-for="(item,index) in closePages" :key="index" @click="changePage(item)" class="page-item page-link"  v-bind:class="{'bg-primary': isCurrent(item),'text-white':isCurrent(item) }">{{item}}</button>
        <button @click="nextPage()" class="btn btn-primary">Next</button>
      </ul>
    </div>

    
  </div>
</template>



<script>
import Repository from "../Api/RepoFactory";
import axios from "axios"
const ProductsRepository = Repository.get("products");
const BrandRepository = Repository.get("brands");
const CategoriesRepository = Repository.get("categories");

export default {
  data(){
   return {
     loading: true, //id of the product (from routing)
     insert:true,

     currentpage:1,
     orderBy:0,    //integer to choose the criteria of ordering
     isAsc:true,
     brandName:"",
     brandSelect:"",

     info:{}, //object to contain the list of products fetched from the db
     

   }
  },

  methods:{
    //fetch a page of products through the repository get method
    async fetchPage(){
      this.info = await ProductsRepository.get(this.currentpage, this.orderBy,this.isAsc,this.brandName);
    },
    //used to update the product page when a filter is applied
    updateData(){
      this.currentpage=1;
      this.fetchPage();
    },
    //used to load all initial needed data. In particular the first page of products, all brands and all categories
    async getData(){ 
      await this.fetchPage();
      let temp = await BrandRepository.getAll();
      this.brands = temp.data
      this.loading = false;
    },
    //increase the current page by one and refresh the data
    nextPage(){
      if(this.currentpage < this.info.data.totalPagesNumber)
        ++ this.currentpage;
       this.fetchPage();
    },
    //decrease the current page by one and refresh the data
    previousPage(){
      if(this.currentpage > 1)
        -- this.currentpage;
       this.fetchPage();    
    },
    //change the current page to a specific one and refresh the data
    changePage(pageNum){
      this.currentpage = pageNum;
      this.fetchPage();
    },
    //changes the ordering of the data
    async selectOrderBy(n){
      this.orderBy==n? this.isAsc = !this.isAsc : this.orderBy= n;
      this.updateData();
    },
    //delete a product and refreshes the page
    async deleteProduct(id){
      if(confirm("are you sure you want to delete this product?"))
      {
        await ProductsRepository.delete(id);
        this.changePage(this.currentpage);
      }
    },
    isCurrent(x){
      return this.currentpage == x 
    }

  },
  computed:{
    //returns the list of products from the info fetched.
    getProducts(){
      return this.info.data.listEntities
    },
    //returns an array with the closest pages to the current one
    closePages(){
      let x=[]
      let p= this.currentpage
      p-2<2? p:x.push(p-2)
      p-1<1? p:x.push(p-1)
      x.push(p);
      p+1>this.info.data.totalPagesNumber? p:x.push(p+1)
      p+2>this.info.data.totalPagesNumber? p:x.push(p+2)
      return x
    },


  },

  async created(){
    await this.getData();
  }
  
}
</script>

<style>
.sortArrow {
  font-size:12px;
  padding-top: 10px;
  padding-bottom: 5px;
}
.hover:hover{
  transform: scale(1.001);
  box-shadow: 0 10px 20px rgba(0,0,0,.12), 0 4px 8px rgba(0,0,0,.06);
}
</style>