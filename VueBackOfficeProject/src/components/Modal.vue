<template>
<div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel">Deleting a Product...</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          Are you sure you want delete this item?
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
          <button type="button" class="btn btn-danger" data-bs-dismiss="modal" @click.stop="deleteProduct()" >Delete</button>
        </div>
      </div>
    </div>
</div>
</template>



<script >

    export default {
        data() {
            return {

            };
        },  
        props:{
        },
        methods: {
            deleteProduct(){
                this.$emit('deleteItem')
            },
        },
        computed:{

        },
        async created() {

        },

    };
</script>