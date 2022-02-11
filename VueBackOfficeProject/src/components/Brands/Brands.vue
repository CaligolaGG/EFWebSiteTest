<template>
<div class="container" v-if="!this.loading">
  <div class="row">
    <div class="col-10">
      <h2> Brands </h2>
    </div> 
    <div class="col">
      <button class="btn btn-outline-primary mx-2" @click="$router.push({path:'/brands/new'})">AddBrand</button>
    </div>
  </div><hr>
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
              <td></td>
                <td>
                  <div class="row">
                  <div class="col">
                    <input class="form-control" type="text" name="" id="" v-model="search" placeholder="BrandName">
                  </div>
                  <div class="col">
                    <button class="btn btn-primary" @click="updateData()">Search</button>
                  </div></div>
                </td><td></td> <td></td>
            </tr>
            </thead>

            <tbody >
                <tr class="bg-light hover" v-for="brand in this.getBrands" :key="brand.brandId" @click.stop="$router.push({path:'/brands/'+brand.brandId})">
                    <td class="col-2" >{{brand.brandId}}</td>
                    <td class="col-4">{{brand.brandName}}</td>
                    <td class="col-4">{{brand.description}}</td>
                    <td class="col-1">
                        <button class="col offset-1 btn btn-outline-secondary bi bi-pencil-square" @click.stop="$router.push({path:'/brands/'+brand.brandId+'/edit'})">  </button>
                        <button class="col btn btn-outline-secondary text-danger bi bi-trash-fill text" @click.stop="Remove(brand.brandId)">  </button>
                    </td>
                </tr>
            </tbody>
        </table>

    </div>

    <ul class="pagination justify-content-center">
        <button @click="previousPage()" class="btn btn-primary ">Previous</button>
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
    search:"",  //variable that holds the  string searched by the user

    currentpage:1,

    info:{}, //object to contain the list of products fetched from the db
     

   }
  },

  methods:{
    //fetch a page of products through the repository get method
    async fetchPage(){
        var error = false

        let temp =  await BrandRepository.getAll(this.currentpage,this.search.trim(),10)
                                        .catch(()=>{alert("no brands found"); error=true});
        if(!error)
          this.info = temp
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
    //check if the index passed to the function is the same as the current page
    isCurrent(x){
      return this.currentpage == x 
    },
    //remove the brand selected
    async Remove(id) {
        if(confirm("are you sure you want to delete this brand?"))
        {
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