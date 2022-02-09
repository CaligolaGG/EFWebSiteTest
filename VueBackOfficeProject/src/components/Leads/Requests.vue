<template>
<div>
    <span> Requests </span>
    <table class="table table-striped table-light ">
        <thead >
          <tr>
            <th scope="col" class="position-relative">Data  </th>
            <th scope="col" class="position-relative" >Product </th>
            <th scope="col" class="position-relative" >Categories </th>
            <th scope="col">  </th>
          </tr>
        </thead></table>
</div>
</template>



<script >

    export default {
        data() {
            return {
                loading: true, //id of the product (from routing)
                insert:true,

                currentpage:1,
                orderBy:0,    //integer to choose the criteria of ordering
                isAsc:true,
                searchByBrand:"",
                searchByProduct:"",

            };
        },
        created() {
        },
        methods: {
            //fetch a page of products through the repository get method
            async fetchPage(){
                //this.info = await RequestRepository.getAll(this.currentpage,this.search,10);
            },
        },
    };
</script>