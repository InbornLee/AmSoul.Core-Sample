<template>

</template>
<script setup>
    import { onMounted, computed, defineComponent, ref } from 'vue'
    import { usePagination } from 'vue-composable';
    import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/vue/solid";
    import { TransitionChild, TransitionRoot, } from "@headlessui/vue";
    onMounted(() => {
    });
    const DataTable = defineComponent({
        components: { ChevronLeftIcon, ChevronRightIcon },
        props: {

        }
    });
    const pageSizes = [10, 20, 50, 100, 500];

</script>