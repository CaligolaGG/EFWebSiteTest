<template>
    <div v-if="!this.loading">{{ $route.params.id }}
        <h1> {{info.productName}} </h1>
        <h2>{{info.brandName}} </h2>
        <li v-for="cat in info.categories" :key="cat.Id">{{cat.name}}</li>

        <div> Requests
            {{info.guestUsersRequestsNumber}}
            {{info.loggedUsersRequestsNumber}}
            <button class="btn btn-primary"> Vedi tutte le richieste per questo prodotto </button>
            cambia routing con filtro prodotto
        </div>
    </div>
</template>

<script>
import Repository from "../../Api/RepoFactory";
import axios from "axios"
const ProductsRepository = Repository.get("products");

const BrandRepository = Repository.get("brands");
const CategoriesRepository = Repository.get("categories");

export default {
    data() {
        return {
            productId:0,
            loading:true,
            
            info:{},

        }    
    },
    methods:{
        async getData(){ 
            let temp= await ProductsRepository.getProduct(this.productId);
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
