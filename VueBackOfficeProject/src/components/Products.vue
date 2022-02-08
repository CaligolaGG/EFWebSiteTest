<template>
  <div class="mx-2">


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
              <option disabled value="">Select a brand</option>
              <option v-for="brand in this.brands" :key="brand.Id"  > {{brand.name}} </option>  
            </select>  
          </td>
          <td></td><td></td><td></td><td></td>
        </tr>
        <tbody >
          <tr v-for="item in this.getProducts" :key="item.Id" class="hover"  @click="$router.replace({path:'/products/'+item.id})">
            <td class="col">{{item.brandName}}</td>
            <td class="col"> <b> {{item.productName}} </b> |  {{item.description}}</td>
            <td class="col-4"><span v-for="(cat, index) in item.categories" :key="index" class="rounded-pill bg-primary text-light ">  <small class="p-1"> {{cat}} </small> </span> </td>
            <td class="col">{{item.price}}</td>
            <td class="col-2">
              <div class="mx-3 d-flex justify-content-end">
              <button class="col-3 offset-1 btn btn-outline-secondary bi bi-pencil-square"></button>
              <button class="col-3 btn btn-outline-secondary text-danger"><i class="bi bi-trash-fill text"></i></button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
      <button @click="previousPage()" class="btn btn-primary mx-1">Previous</button>
      <button v-for="(item,index) in closePages" :key="index" @click="changePage(item)">{{item}}</button>
      <button @click="nextPage()" class="btn btn-primary">Next</button>
    </div>

    <br><br><br>
    <form  id="insert" v-if="insert" v-on:submit.prevent="submitForm()">
      insert new product
      <div class="form-group mb-2">
        <label for="pname">Name</label>
        <input type="text" name="pname" id="" class="form-control" v-model="form.Name">
      </div>
      <div class="form-group mb-2">
        <label for="desc">Description</label>
        <input type="textarea" class="form-control" name="desc" v-model="form.Description">
      </div>
      <div class="form-group mb-2">
        <label for="sdesc">ShortDescription</label>
        <input type="textarea" class="form-control" name="sdesc" v-model="form.ShortDescription">
      </div>
      <div class="form-group mb-2">
        <label for="price">Price</label>
        <input type="number" class="form-control" name="price" v-model="form.Price">
      </div>
      <div>
        BrandId 
        <select name="" id="" class="form-select m-1 "  v-model="form.BrandId">
          <option disabled value="">Please select one</option>
          <option v-for="brand in this.brands" :key="brand.id" v-bind:value="brand.id" > {{brand.name}}</option>  
        </select>  
        Categories 
        <select name="categories" id="" class="form-select m-1" v-model="catsSelect" multiple>
          <option disabled value="">Please select one</option>
          <option v-for="cat in this.categories" :key="cat.Id" v-bind:value="cat.id"> {{cat.name}} </option>
        </select>
      </div>
      <button type="submit" class="btn btn-primary mt-2">Submit</button>
    </form>
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
     loading: true,
     insert:true,

     currentpage:1,
     orderBy:0,
     isAsc:true,
     brandName:"",
     brandSelect:"",

     info:{},
     brands:{},
     categories: {},

     form:
     {
       Name:"",
       Description:"",
       ShortDescription:"",
       Price:0,
       BrandId:null,
     },
    catsSelect:[]


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
      let cats = await CategoriesRepository.get();
      this.categories = cats.data;
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
      this.currentpage =pageNum;
      this.fetchPage();
    },
    //changes the ordering of the data
    async selectOrderBy(n){
      this.orderBy==n? this.isAsc = !this.isAsc : this.orderBy= n;
      this.updateData();
    },
    //insert a new product in the db by calling the specific repository function
    submitForm(){
      if(this.catsSelect.length == 0)
        ProductsRepository.create(this.form)
      else
      {
        let productWithCats = {Product : this.form, Categories: this.catsSelect}
        ProductsRepository.createWithCats(productWithCats)
      }
      console.log(this.form)
      console.log(this.catsSelect)
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
      p+1>this.info.data.totalPagesNumber? p:x.push(p+1)
      p+2>this.info.data.totalPagesNumber? p:x.push(p+2)
      return x
    }

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