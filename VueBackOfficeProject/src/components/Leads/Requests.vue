<template>
    <div v-if="!this.loading" class="container">
        <h2> Requests </h2>
        <hr>
        <div class="row my-1">
                  <div class="alert alert-danger" role="alert" v-bind:class="{'d-none':!alertActive}" >
                    No Leads Found
                </div>
            <div class="col-2">
                Select a Brand
            </div>
            <div class="col-5">
            <select  name="" id="" class="form-select m-1"  v-model="searchByBrand" @change="fetchPage()"  >
                <option default value=0>No Brand</option>
                <option v-for="brand in this.brands" :key="brand.Id" v-bind:value='brand.id'> {{brand.name}} </option>  
            </select>
            </div>
        </div>

        <table class="table table-striped table-light ">
            <thead >
            <tr>
                <th class="position-relative">UserName  </th>
                <th class="position-relative" >Product </th>
                <th class="position-relative" >PhoneNumber </th>
                <th class="position-relative" >Email </th>
                <th class="position-relative" >RequestText </th>
                <th class="position-relative hoverV2" @click="ordering" >Date 
                <i  class="bi bi-caret-down-fill position-absolute bottom-0 end-0 sortArrow" v-bind:class="{'text-primary':!isAsc}"> </i> 
                <i class="bi bi-caret-up-fill position-absolute top-0 end-0 sortArrow" v-bind:class="{'text-primary':isAsc}" ></i>
                </th>
            </tr>
            <tr>
                <th></th>
                <th >
                    <input v-debounce:1000ms="searchDebounced" class="form-control" type="text"  placeholder="Product Name" name="" id="" v-model="searchByProduct">
                </th><th colspan="4"></th> 
            </tr>
            </thead>
            <tbody>
                <tr v-for="lead in this.getLeads" :key="lead.id" class="hover" @click.stop="$router.push({path:'/leads/'+lead.id})">
                    <td>{{lead.userName}}</td>
                    <td>{{lead.productName}}</td>
                    <td>{{lead.phoneNumber}}</td>
                    <td>{{lead.email}}</td>
                    <td>{{lead.requestText}}</td>
                    <td> {{convertToDate(lead.date)}}</td>
                </tr>    
            </tbody>    
        </table>
        <Paging @changePage="fetchPage" v-bind:totalPagesNumber="info.data.totalPagesNumber"/> 
    </div>
</template>

<script >

import Repository from "../../Api/RepoFactory";
import Vue from 'vue'
import vueDebounce from 'vue-debounce'
import Paging from '../Pagination.vue'
const BrandRepository = Repository.get("brands");
const LeadsRepository = Repository.get("leads");


Vue.use(vueDebounce)

    export default {
        data() {
            return {
                loading: true, //id of the product (from routing)
                insert:true,
                info:{},        //info fetched from the api
                brands:{},      //list of all brand (id + name)

                currentpage:1,
                orderBy:0,    //integer to choose the criteria of ordering
                isAsc:false,
                searchByBrand:0,
                searchByProductId:0,
                searchByProduct:null,
                productName:null,

                alertActive:false

            };
        },  
        components:{
            Paging
        },

        methods: {
            //fetch a page of products through the repository get method
            async fetchPage(pageNum = 1){
                var error = false
                if(!this.searchByProductId)
                {
                    if(this.searchByProduct )
                         this.searchByProduct = this.searchByProduct.trim()
                    let temp = await LeadsRepository.getPage(pageNum,this.searchByBrand,this.searchByProduct, this.isAsc, 10,0)
                    .catch(()=>{this.alertActive=true; error=true; })
                    if(!error) 
                    {
                        this.alertActive = false
                        this.info = temp;
                    }
                }
                else
                {
                    let temp = await LeadsRepository.getPage(pageNum,this.searchByBrand,undefined, this.isAsc,10,this.searchByProductId)
                    .catch(()=>{this.alertActive = true; error=true})
                    if(!error)
                    {
                        this.alertActive = false
                       this.info = temp
                    }
                }
            },
            searchDebounced(){if (this.searchByProduct.length > 3 || this.searchByProduct.length ==0) this.fetchPage()},
            
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
            convertToDate(date){
                return new Date(date).toLocaleDateString("it")
            },



        },
        computed:{
            //returns the list of products from the info fetched.
            getLeads(){
                return this.info.data.listEntities
            },
        },
        async created() {
            this.searchByProductId = this.$route.params.productId
            this.searchByBrand = this.$route.params.brandId
            await this.getData();
        },

    };
</script>