<template>
<div class="position-fixed bottom-0 end-0 p-3" style="z-index: 11">
  <div id="liveToast" class="toast  border border-success" role="alert" aria-live="assertive" aria-atomic="true">
    <div class="toast-header border-bottom-1 border-success ">
      <strong class="me-auto text-success">{{itemType}} deleted successfully</strong>
      <small>1 sec ago</small>
      <button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>
    </div>
    <div class="toast-body">
      {{deleteItemName}} is no more
    </div>
  </div>
</div>
</template>

<script>

export default ({

    props:{
        deleteItemName:String,
        itemType:String
    }

})
</script>
