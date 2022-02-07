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
          </tr>
        </thead>
        <tr class="bg-light">
          <td> 
            <select name="" id="" class="form-select m-1 "  v-model="brandName" @change="updateData()">
              <option disabled value="">Please select one</option>
              <option v-for="(brand,index) in this.brands" :key="index"  > {{brand}} </option>  
            </select>  
          </td>
          <td></td><td></td><td></td>
        </tr>
        <tbody>
          <tr v-for="item in this.getProducts" :key="item.Id" class="hover">
            <td>{{item.brandName}}</td>
            <td> <b> {{item.productName}} </b> |  {{item.description}}</td>
            <td><span v-for="(cat, index) in item.categories" :key="index" class="rounded-pill bg-primary text-light ">  <small class="p-1"> {{cat}} </small> </span> </td>
            <td>{{item.price}}</td>
          </tr>
        </tbody>
      </table>
      <button @click="previousPage()" class="btn btn-primary mx-1">Previous</button>
      <button v-for="(item,index) in closePages" :key="index" @click="changePage(item)">{{item}}</button>
      <button @click="nextPage()" class="btn btn-primary">Next</button>
    </div>


    <form  id="insert" v-if="insert">
      insert new product
      <div class="form-group mb-2">
        <label for="pname">Name</label>
        <input type="text" name="pname" id="" class="form-control">
      </div>
      <div class="form-group mb-2">
        <label for="desc">Description</label>
        <input type="textarea" class="form-control" name="desc">
      </div>
      <div class="form-group mb-2">
        <label for="sdesc">ShortDescription</label>
        <input type="textarea" class="form-control" name="sdesc">
      </div>
      <div class="form-group mb-2">
        <label for="price">Price</label>
        <input type="number" class="form-control" name="price">
      </div>
      <div>
        BrandId 
        <select name="" id=""  class="form-control mb-2">
              <option value=""> V-for </option>
              <option value="">1</option>
              <option value="">2</option>
        </select>
        Categories 
        <select name="" id=""  class="form-control mb-2">
              <option value=""> V-for </option>
              <option value="">1</option>
              <option value="">2</option>
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

export default {
 data(){
   return {
     loading: true,
     insert:false,

     currentpage:1,
     orderBy:0,
     isAsc:true,
     brandName:"",

     info:{},

     brands:{},

   }
  },

  methods:{
    async fetchPage(){
      this.info = await ProductsRepository.get(this.currentpage, this.orderBy,this.isAsc,this.brandName);
    },
    updateData(){
      this.currentpage=1;
      this.fetchPage();
    },
    async getData(){ 
      await this.fetchPage()
      let temp = await BrandRepository.getAllNames();
      this.brands = temp.data
      this.loading = false
    },
    nextPage(){
      if(this.currentpage < this.info.data.totalPagesNumber)
        ++ this.currentpage;
       this.fetchPage()
    },
    previousPage(){
      if(this.currentpage > 1)
        -- this.currentpage;
       this.fetchPage()      
    },
    changePage(pageNum){
      this.currentpage =pageNum;
      this.fetchPage();
    },
    async selectOrderBy(n){
      this.orderBy==n? this.isAsc = !this.isAsc : this.orderBy= n;
      this.updateData();
    },
    
  },
  computed:{
    getProducts(){
      return this.info.data.listEntities
    },
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