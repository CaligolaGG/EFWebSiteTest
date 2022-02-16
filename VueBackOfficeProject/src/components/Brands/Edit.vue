<template>
<div class="container" v-if="!this.loading">

    <form  v-if="!this.loading" id="insert" v-on:submit.prevent="submitForm()">
            <h2> Update this Brand </h2>
            <div class="form-group mb-2">
                <div class="alert alert-danger alert-dismissible fade show" role="alert" v-bind:class="{'d-none':!alertActive}">
                    <button class="btn bg-danger text-white bi bi-x-lg" @click="alertActive = false" type="button"  data-dismiss="alert" ></button>
                    {{alertText}}
                </div>
                
                <label for="pname">Name</label>
                <input type="text" name="pname" id="" class="form-control"  maxlength="50" v-model="info.data.brandName" >
            </div>
            <div class="form-group mb-2">
                <label for="desc">Description</label>
                <input type="textarea" class="form-control" name="desc"  maxlength="50" v-model="info.data.description">
            </div>

            <button @keyup.enter="submitForm()" type="submit" class="btn btn-primary mt-2">Submit</button>
        </form>

</div>
</template>

<script>

import Repository from "../../Api/RepoFactory";
//const ProductsRepository = Repository.get("products");
const BrandRepository = Repository.get("brands");

export default {
    data() {
        return {
            id:0,           //id of the product (from routing)
            loading:true,   //boolean to know if the data has been fetched yet
            info:{},        //object that contains the info of the brand fetched
            error:null,
            alertActive:false,
            alertText:""
        }    
    },
    methods:{
        //get the brand data by calling the specific api
        async getData(){ 
            this.info = await BrandRepository.getById(this.id);
            this.loading=false;
        },
        //submit the updated info from the form
        async submitForm(){
            let id = await BrandRepository.update(this.info.data).catch( (response)=> this.error = response);
            if(this.error && this.error.response.status == 404)
                {
                    this.alertActive=true
                    this.alertText = "Brand Name already taken. " + this.error.response.data
                }
            else
                this.$router.push({path:'/brands/'+id.data.id})
            //else go to brand detail

        }

    },
    async created(){
        this.id = this.$route.params.id;
        await this.getData();
    }
    
}
</script>
