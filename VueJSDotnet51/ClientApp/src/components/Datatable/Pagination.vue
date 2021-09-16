<template>
    <div class="pagination">
        <div class="flex-1 flex justify-between sm:hidden pagination">
            <button @click="prev" class="paginationbtn">
                <ChevronLeftIcon class="h-5 w-5" aria-hidden="true" />
            </button>
            <span class="select-none">{{ currentPage+' / '+lastPage }}</span>
            <button @click="next" class="ml-3 paginationbtn">
                <ChevronRightIcon class="h-5 w-5" aria-hidden="true" />
            </button>
        </div>
        <div class="hidden sm:flex-1 sm:flex sm:items-center sm:justify-between">
            <div>
                <p class="text-xs text-gray-700">
                    <span class="mr-1">Showing</span>
                    <span class="mr-1">{{offset+1}}</span>
                    <span class="mr-1">to</span>
                    <span class="mr-1">{{offset+pageSize}}</span>
                    <span class="mr-1">of</span>
                    <span class="mr-1">{{total}}</span>
                    <span class="mr-1">results</span>
                    <select v-model="pageSize" class="inline-flex w-14 py-2 pl-2 pr-1 border border-gray-300 bg-white rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-xs">
                        <option v-for="(item , index) in props.pageSizes" :key="index" :value="item">{{ item }}</option>
                    </select>
                    <span class="mr-1">rows per page</span>
                </p>
            </div>
            <div>
                <nav class="paginationNav" aria-label="Pagination">
                    <span @click="prev" class="paginationbtn">
                        <span class="sr-only">Previous</span>
                        <ChevronLeftIcon class="h-5 w-5" aria-hidden="true" />
                    </span>
                    <!-- Current: "z-10 bg-indigo-50 border-indigo-500 text-indigo-600", Default: "bg-white border-gray-300 text-gray-500 hover:bg-gray-50" -->
                    <span @click="first" class="paginationbtn" :class="{ 'active' : currentPage === 1 }">
                        1
                    </span>
                    <span v-if="currentPage>4" @click="currentPage-=3" class="paginationbtn">
                        ...
                    </span>
                    <span v-for="page in pageRender" :key="page" @click="currentPage=page" class="paginationbtn" :class="{ 'active' : page === currentPage }">
                        {{ page }}
                    </span>
                    <span v-if="currentPage<lastPage-3" @click="currentPage+=3" class="paginationbtn">
                        ...
                    </span>
                    <span @click="last" class="paginationbtn" :class="{ 'active' : currentPage === lastPage }">
                        {{ lastPage }}
                    </span>
                    <span @click="next" class="paginationbtn">
                        <span class="sr-only">Next</span>
                        <ChevronRightIcon class="h-5 w-5" aria-hidden="true" />
                    </span>
                </nav>
            </div>
        </div>
    </div>
</template>
<script setup>
    import { computed, defineComponent } from 'vue'
    import { usePagination } from 'vue-composable'
    const Pagination = defineComponent({
        name: "Pagination",
        components: { usePagination },
        props: {
            pageSizes: { type: Array, required: true, default: [10, 20, 50, 100, 500] },
            currentPage: { type: Number, required: true, default: 1 },
            pageSize: { type: Number, required: true, default: 20 },
            total: { type: Number, required: true },
        },
        emits: ["changed"],
        setup(props, { emit }) {
            const { pageSize, total, currentPage, offset, lastPage, next, prev, first, last } = usePagination({
                currentPage: props.currentPage,
                pageSize: props.pageSize,
                total: props.total
            });

            const pageRender = computed(() => {
                let arr = Array.from(Array(lastPage.value - 2), (v, k) => k + 2);
                let result = currentPage.value < 5 ? arr.slice(0, 4) : (currentPage.value > lastPage.value - 4 ? arr.slice(-4) : arr.slice(currentPage.value - 3, currentPage.value));
                return result;
            });
        }
    });
</script>