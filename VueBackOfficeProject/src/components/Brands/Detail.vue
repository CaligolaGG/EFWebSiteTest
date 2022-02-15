<template>
    <div class="container" v-if="!this.loading"> 
        <h1> {{info.brandName}} </h1>
        <h2>{{info.description}} </h2>

        <p>Categories associated to this Brand's Products</p>
        <div class="row mx-1 ">
            <div  class="list-group-item col-4 border rounded" v-for="cat in info.listCategories" :key="cat.categoryId">
                {{cat.categoryName}} ({{cat.categoryId}})
            </div>
        </div>
    <hr>
    Requests {{info.numberRequests}}
    <table class="table table-striped ">
        <thead class="bg-mygreen text-white">
            <tr>
                <th class="border border-success">Id</th>
                <th class="border border-success">Product Name</th>
                <th class="border border-success">Request Number</th>
            </tr>
        </thead>
        <tbody>
        <tr class="hover" v-for="prod in productPage" :key="prod.productId" @click="$router.push({path:'/products/'+prod.productId})">
            <td class="border border-success">{{prod.productId}} </td>
            <td class="border border-success">{{prod.productName}}</td>
            <td class="border border-success">{{prod.productRequestNumber}}</td>
        </tr>
        </tbody>

    </table>
    <ul class="pagination justify-content-center">
        <button @click="previousPage()" class="btn btn-primary mx-1">Previous</button>
        <button @click="nextPage()" class="btn btn-primary">Next</button>
    </ul>
    </div>
</template>

<script>
import Repository from "../../Api/RepoFactory";
const BrandRepository = Repository.get("brands");

export default {
    data() {
        return {
            id:0,           //id of the product (from routing)
            loading:true,   //boolean to know if the data has been fetched yet
            info:{},        //object that contains the info of the product fetched
            currentPage:1,
        }    
    },
    methods:{
        //get the product detail data by calling the specific api
        async getData(){ 
            let temp= await BrandRepository.getBrand(this.id);
            this.info = temp.data;
            this.loading = false;
        },
        previousPage(){
            if(this.currentPage > 1)
                this.currentPage--;
        },
        nextPage(){
            if(this.currentPage + 1 <= this.info.listProducts.length /9)
                this.currentPage ++;
        },
    },
    computed:{
        productPage(){
            return this.info.listProducts.slice(this.currentPage * 10 - 10 ,10 * this.currentPage)
        }
    },
    async created(){
        this.id = this.$route.params.id;
        await this.getData();
    }
    
}
</script>

<style scoped> 
.bg-mygreen{
    background-color: #5cc51f;
 }
.table-striped>tbody>tr:nth-child(odd)>td, 
.table-striped>tbody>tr:nth-child(odd)>th {
   background-color: #c2ff1c; 
 };

</style>