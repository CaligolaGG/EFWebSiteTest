<template>
    <div class="container" v-if="!this.loading" >
        <h3> {{info.productName}} by
            {{info.brandName}} </h3>

        <p class="mt-4 "> <b> Categories associated to the product: </b></p>
        <div class="row mx-1">
            <ul class=" list-group">
                <li class="list-group-item" v-for="cat in info.categories" :key="cat.Id">{{cat.name}}</li>
            </ul>
        </div>

        <p class="mt-5"> <b> Leads for this product: </b></p>
        <div class="row"> 
            <div class="col-12">
                <b> {{info.guestUsersRequestsNumber + info.loggedUsersRequestsNumber }} InfoRequest </b> for this product
                <b> {{info.guestUsersRequestsNumber}}</b> from <b> Guest Users </b>|
                <b>{{info.loggedUsersRequestsNumber}}</b> from <b> Logged Users </b> 
            </div>
        </div>
        <button class="btn btn-primary my-2" @click="$router.push({name: 'leads', params: { productName: info.productName, brandId: info.brandId  }});"> Vedi tutte le richieste per questo prodotto </button>

        <!--
        <div class="row">
            <li class="list-group-item" v-for="req in info.requests" :key="req.Id">User {{req.fullName}} --- {{req.repliesCount}} replies</li>
        </div>-->

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
