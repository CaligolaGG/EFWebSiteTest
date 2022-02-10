<template>
    <div class="container">
        <form  v-if="!this.loading" id="insert" v-on:submit.prevent="submitForm()">
            add Brand
            <div class="form-group mb-2">
                <label for="pname">Name</label>
                <input type="text" name="pname" id="" class="form-control"  maxlength="50" v-model="brand.BrandName">
            </div>
            <div class="form-group mb-2">
                <label for="desc">Description</label>
                <input type="textarea" class="form-control" name="desc"  maxlength="50" v-model="brand.Description">
            </div>

            <div class="form-group mb-2">
                <label for="desc">Email</label>
                <input type="textarea" class="form-control" name="desc"  maxlength="50" v-model="account.Email">
            </div>
            <div class="form-group mb-2">
                <label for="desc">Password</label>
                <input type="password" class="form-control" name="desc"  maxlength="50" v-model="account.Password">
            </div>

            <button type="submit" class="btn btn-primary mt-2">Submit</button>
            <add-product v-for="n in 0" :key="n"  @update="onChildUpdate()"></add-product>
        </form>


        <div v-for="n in this.numProducts" :key="n" class="my-3">
            <form  id="insert" v-on:submit.prevent="submitForm()" class="bg-light my-2 border" >
                {{n}}
            insert new product
            <div class="form-group mb-2">
                <label for="pname">Name</label>
                <input type="text" name="pname" id="" class="form-control"  maxlength="50" v-model="bundles[n-1].Product.Name">
            </div>
            <div class="form-group mb-2">
                <label for="desc">Description</label>
                <input type="textarea" class="form-control"  maxlength="50" name="desc" v-model="bundles[n-1].Product.Description">
            </div>
            <div class="form-group mb-2">
                <label for="sdesc">ShortDescription</label>
                <input type="textarea" class="form-control"   maxlength="20" name="sdesc" v-model="bundles[n-1].Product.ShortDescription">
            </div>
            <div class="form-group mb-2">
                <label for="price">Price</label>
                <input type="number" class="form-control" name="price" v-model.number="bundles[n-1].Product.Price">
            </div>
            <div class="form-group mb-2">

                Categories 
                <select name="categories" id="" class="form-select m-1" v-model="bundles[n-1].Categories" multiple>
                <option  value="">Please select one</option>
                <option v-for="cat in categories" :key="cat.Id" v-bind:value="cat.id"> {{cat.name}} </option>
                </select>
            </div>
            </form>
        </div>
        <button @click="addProduct()" class="btn btn-primary mt-2">Add Product</button>

</div>
</template>



<script>

import Repository from "../../Api/RepoFactory";
import addProduct from "../Products/ProductForm.vue"
//const ProductsRepository = Repository.get("products");
const BrandRepository = Repository.get("brands");
const CategoriesRepository = Repository.get("categories");


export default {
    data() {
        return {
            id:0,       //id of the product (from routing)
            loading:true, //boolean to know if the data has been fetched yet
            info:{},      //object that contains the info of the brand fetched
            numProducts: 0,
            

            account:{
                Email:"",
                Password:"",
                AccountType:2
            },
            brand:{
                BrandName:"",
                Description:"",
                isDeleted:0
            },
            categories: {}, //object to contains a list of cateories 

            bundles:[],

        }    
    },
    methods:{
        //get the brand data by calling the specific api
        async getData(){ 
            let cats = await CategoriesRepository.get();
            this.categories = cats.data;
            this.loading=false;
        },
        async submitForm(){
            var id = await BrandRepository.create( {Brand : this.brand , ProductsCategs: this.bundles, Account:this.account});
            this.$router.push({path:'/brands/'+id.data})
        },
        addProduct(){
            this.numProducts++;
            this.bundles.push(new bundle())
            console.log(this.bundles)
        },


    },
    components:{
        addProduct
    },
    async created(){
        this.id = this.$route.params.id;

        await this.getData();
    }
    
}


var bundle = function(){
            this.Product=//object to hold the data inserted in the form that has to be passed to the api
            {
                Name:"",
                Description:"",
                ShortDescription:"",
                Price:0
            },
            this.Categories=[] //array to hold the list of classes selected for the product to insert 
}


</script>
