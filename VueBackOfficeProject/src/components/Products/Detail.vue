<template>
    <div class="container" v-if="!this.loading" >
        <h3> {{info.productName}} by
            {{info.brandName}} </h3>

        <p class="mt-4 "> <b> Categories associated to the product: </b></p>
        <div class="row border bg-light">
            <li v-for="cat in info.categories" :key="cat.Id">{{cat.name}}</li>
        </div>

        <p class="mt-5"> <b> Leads for this product: </b></p>
        <div class="row"> 
            <div class="col-12">
            {{info.guestUsersRequestsNumber + info.loggedUsersRequestsNumber }} InfoRequest for this product
            {{info.guestUsersRequestsNumber}}  of which from Guest Users
            {{info.loggedUsersRequestsNumber}} from Logged Users
            </div>
        </div>

        <div class="row border bg-light">
            <li v-for="req in info.requests" :key="req.Id">User {{req.fullName}} --- {{req.repliesCount}} replies</li>
        </div>

        <button class="btn btn-primary mt-2" > Vedi tutte le richieste per questo prodotto </button>

    </div>
</template>

<script>
import Repository from "../../Api/RepoFactory";
const ProductsRepository = Repository.get("products");


export default {
    data() {
        return {
            productId:0,  //id of the product (from routing)
            loading:true, //boolean to know if the data has been fetched yet
            
            info:{},      //object that contains the info of the product fetched

        }    
    },
    methods:{
        //get the product detail data by calling the specific api
        async getData(){ 
            let temp= await ProductsRepository.getProductDetail(this.productId);
            this.info = temp.data;
            this.loading = false;
        },

    },
    async created(){
        this.productId = this.$route.params.id;
        await this.getData();
    }
    
    
}
</script>
