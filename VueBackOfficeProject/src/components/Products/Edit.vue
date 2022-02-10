<template> 
    <div class="container" v-if="!this.loading">{{ $route.params.id }}
        <form  v-if="!this.loading" id="insert" v-on:submit.prevent="submitForm()">
            update product
            <div class="form-group mb-2">
                <label for="pname">Name</label>
                <input type="text" name="pname" id="" class="form-control"  maxlength="50" v-model="info.product.name">
            </div>
            <div class="form-group mb-2">
                <label for="desc">Description</label>
                <input type="textarea" class="form-control" name="desc"  maxlength="50" v-model="info.product.description">
            </div>
            <div class="form-group mb-2">
                <label for="sdesc">ShortDescription</label>
                <input type="textarea" class="form-control" name="sdesc"  maxlength="20" v-model="info.product.shortDescription">
            </div>
            <div class="form-group mb-2">
                <label for="price">Price</label>
                <input type="number" class="form-control" name="price" v-model="info.product.price">
            </div>
            <div>
                Brand
                <select name="" id="" class="form-select m-1 "  v-model="info.product.brandId">
                <option disabled value="">Please select one</option>
                <option v-for="brand in this.brands" :key="brand.id" v-bind:value="brand.id" > {{brand.name}}</option>  
                </select>  
                Categories {{info.categories}}
                <select name="categories" id="" class="form-select m-1" v-model="info.categories" multiple>
                <option  value="">Please select one</option>
                <option v-for="cat in this.categories" :key="cat.Id" v-bind:value="cat.id"> {{cat.name}} </option>
                </select>
            </div>
            <button type="submit" class="btn btn-primary mt-2">Submit</button>
        </form>
    </div>
</template>

<script>
import Repository from "../../Api/RepoFactory";
const ProductsRepository = Repository.get("products");

const BrandRepository = Repository.get("brands");
const CategoriesRepository = Repository.get("categories");

export default {
    data() {
        return {
            productId:0,  //id of the product (from routing)
            loading:true, //boolean to know if the data has been fetched yet
            
            info:{},      //object that contains the info of the product fetched
            
            catsSelect:[] //array to hold the list of classes selected for the product to insert 
        }    
    },
    methods:{
        //get the product data by calling the specific api
        async getData(){ 
            let temp= await ProductsRepository.getProduct(this.productId);
            this.info = temp.data;

            let c = this.info.categories.slice()
            this.info.categories = [];
            for(let i of c)
                this.info.categories.push(i.id);

            let cats = await CategoriesRepository.get();
            let temp3 = await BrandRepository.get();
            this.brands = temp3.data
            this.categories = cats.data;
            this.loading = false;
        },
        submitForm(){
            let newProduct = { Product : this.info.product, Categories : this.info.categories }
            console.log(newProduct);
            ProductsRepository.update(newProduct);
        }

    },
    async created(){
        this.productId = this.$route.params.id;
        await this.getData();
    }
    
}
</script>
