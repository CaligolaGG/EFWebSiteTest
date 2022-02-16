<template>
    <div class="container" v-if="!this.loading" >
        <h3> {{info.productName}} by
            {{info.brandName}} </h3>

        <p class="mt-4 "> <b> Categories associated to the product: </b></p>
        <div class="row mx-1 ">
            <div class="list-group-item  col-4 border rounded" v-for="cat in info.categories" :key="cat.Id">{{cat.name}}</div>
        </div>

        <p class="mt-5"> <b> Leads for this product: </b></p>
        <div class="row"> 
            <div class="col-12">
                <b> {{info.guestUsersRequestsNumber + info.loggedUsersRequestsNumber }} InfoRequest </b> for this product
                <b> {{info.guestUsersRequestsNumber}}</b> from <b> Guest Users </b>|
                <b>{{info.loggedUsersRequestsNumber}}</b> from <b> Logged Users </b> 
            </div>
        </div>
        <hr class="text-warning">
        <button v-if="this.info.guestUsersRequestsNumber + this.info.loggedUsersRequestsNumber " class="btn btn-outline-primary my-2" @click="$router.push({name: 'leads', params: { productId: info.productId, brandId: info.brandId  }});"> Vedi tutte le richieste per questo prodotto </button>

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
        this.productId = this.$route.params.id; //id of the current product from routing
        await this.getData();
    }
    
    
}
</script>
