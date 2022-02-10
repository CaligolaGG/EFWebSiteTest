<template>
    <div class="container" v-if="!this.loading">{{ $route.params.id }}
        <h1> {{info.brandName}} </h1>
        <h2>{{info.description}} </h2>
        <li v-for="cat in info.listCategories" :key="cat.categoryId">{{cat.categoryName}}</li>

<hr>
            Requests {{info.numberRequests}}
        <li v-for="prod in productPage" :key="prod.productId">{{prod.productId}}  ---  {{prod.productName}}  --- {{prod.productRequestNumber}}</li>
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
            id:0,  //id of the product (from routing)
            loading:true, //boolean to know if the data has been fetched yet
            info:{},      //object that contains the info of the product fetched
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