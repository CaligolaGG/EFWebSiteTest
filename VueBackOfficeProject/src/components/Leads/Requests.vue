<template>
<div v-if="!this.loading" class="container">
    <span> Requests </span>
    <table class="table table-striped table-light ">
        <thead >
          <tr>
            <th class="position-relative">UserName  </th>
            <th class="position-relative" >Product </th>
            <th class="position-relative" >PhoneNumber </th>
            <th class="position-relative" >Email </th>
            <th class="position-relative" >RequestText </th>
            <th class="position-relative" @click="ordering" >Date 
              <i  class="bi bi-caret-down-fill position-absolute bottom-0 end-0 sortArrow" > </i> <!--v-bind:class="{'text-primary':selectArrow(1,false)}"-->
              <i class="bi bi-caret-up-fill position-absolute top-0 end-0 sortArrow"  ></i>

            </th>
          </tr>
          <tr>
             <th colspan="2">
                  <input type="text" name="" id="" v-model="this.productName"> <button class="btn btn-primary" >SearchProduct</button>
              </th>
              <th colspan="2">
                    <select name="" id="" class="form-select m-1 "  v-model="searchByBrand" @change="updateData()">
                        <option value="">Select a brand</option>
                        <option v-for="brand in this.brands" :key="brand.Id" v-bind:value='brand.id'> {{brand.name}} </option>  
                  </select>
              </th>
              <th></th>
              <th></th>
          </tr>
        </thead>
        <tbody>
            <tr v-for="lead in this.getLeads" :key="lead.id" class="hover" @click.stop="$router.push({path:'/leads/'+lead.id})">
                <td>{{lead.userName}}</td>
                <td>{{lead.productName}}</td>
                <td>{{lead.phoneNumber}}</td>
                <td>{{lead.email}}</td>
                <td>{{lead.requestText}}</td>
                <td>{{lead.date}}</td>
            </tr>    
        </tbody>    
    </table>


    <ul class="pagination justify-content-center">
        <button @click="previousPage()" class="btn btn-primary mx-1">Previous</button>
        <button v-for="(item,index) in closePages" :key="index" @click="changePage(item)" class="page-item page-link"  v-bind:class="{'bg-primary': isCurrent(item),'text-white':isCurrent(item) }">{{item}}</button>
        <button @click="nextPage()" class="btn btn-primary">Next</button>
    </ul>
</div>
</template>



<script >

import Repository from "../../Api/RepoFactory";
//const ProductsRepository = Repository.get("products");
const BrandRepository = Repository.get("brands");
const LeadsRepository = Repository.get("leads");


    export default {
        data() {
            return {
                loading: true, //id of the product (from routing)
                insert:true,
                info:{},
                brands:{},

                currentpage:1,
                orderBy:0,    //integer to choose the criteria of ordering
                isAsc:false,
                searchByBrand:0,
                searchByProduct:0,
                brandName:"",
                productName:null

            };
        },

        methods: {
            //fetch a page of products through the repository get method
            async fetchPage(){
                this.info = await LeadsRepository.getPage(this.currentpage,this.searchByBrand,this.searchByProduct,this.isAsc,10);
            },
            async updatePage(){
                this.currentpage = 1;
                await this.fetchPage();
            },
            async getData(){
                await this.fetchPage();
                let temp= await BrandRepository.get();
                this.brands = temp.data;
                this.loading = false;
            },
            async ordering(){
                this.isAsc = !this.isAsc;
                await this.updatePage();
            },
            updateData(){
                this.currentpage=1;
                this.fetchPage();
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

        },
        computed:{
            //returns the list of products from the info fetched.
            getLeads(){
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
        async created() {
            await this.getData();
        },
    };
</script>