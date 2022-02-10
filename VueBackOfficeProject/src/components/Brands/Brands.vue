<template>
<div class="container" v-if="!this.loading">
    <button class="btn btn-primary" @click="$router.push({path:'/brands/new'})">AddBrand</button>
    <span> Brands </span>
    <div >
        <table class="table table-striped table-light  ">
            <thead >
            <tr>
                <th scope="col" class="position-relative" >BrandId </th>
                <th scope="col" class="position-relative" >BrandName </th>
                <th scope="col" class="position-relative" >description </th>
                <th scope="col">  </th>
            </tr>
            <tr class="bg-light">
                <td>
                    <input type="text" name="" id="" v-model="search" placeholder="BrandName">
                    <button class="btn btn-primary" @click="updateData()">Search</button>
                </td><td></td><td></td><td></td>
            </tr>
            </thead>

            <tbody >
                <tr class="bg-light hover" v-for="brand in this.getBrands" :key="brand.brandId" @click.stop="$router.push({path:'/brands/'+brand.brandId})">
                    <td >{{brand.brandId}}</td>
                    <td >{{brand.brandName}}</td>
                    <td >{{brand.description}}</td>
                    <td >
                        <button class="btn btn-info" @click.stop="$router.push({path:'/brands/'+brand.brandId+'/edit'})"> Edit </button>
                        <button class="btn btn-danger" @click.stop="Remove(brand.brandId)"> Delete </button>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>

    <ul class="pagination justify-content-center">
        <button @click="previousPage()" class="btn btn-primary mx-1">Previous</button>
        <button v-for="(item,index) in closePages" :key="index" @click="changePage(item)" class="page-item page-link"  v-bind:class="{'bg-primary': isCurrent(item),'text-white':isCurrent(item) }">{{item}}</button>
        <button @click="nextPage()" class="btn btn-primary">Next</button>
    </ul>
</div>
</template>


<script lang="js">

import Repository from "../../Api/RepoFactory";
const BrandRepository = Repository.get("brands");

export default {
  data(){
   return {
    loading: true, //id of the product (from routing)
    insert:true,
    search:"",

    currentpage:1,

    info:{}, //object to contain the list of products fetched from the db
     

   }
  },

  methods:{
    //fetch a page of products through the repository get method
    async fetchPage(){
        this.info = await BrandRepository.getAll(this.currentpage,this.search,10);
    },
    //used to update the product page when a filter is applied
    async updateData(){
      this.currentpage=1;
      await this.fetchPage();
    },
    //used to load all initial needed data. In particular the first page of products, all brands and all categories
    async getData(){ 
      await this.fetchPage();
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
    isCurrent(x){
      return this.currentpage == x 
    },

    async Remove(id) {
        if(confirm("are you sure you want to delete this brand?"))
        {
            alert(id)
            await BrandRepository.delete(id);
            this.changePage(this.currentpage);
        }
    },

  },
  computed:{
    //returns the list of products from the info fetched.
    getBrands(){
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