<template>
    <div class="bg-gray-100">
        <br />
        <div class="card mx-4">
            <button type="button" class="inline-flex btn btn-secondary" @click="modalOpen=true,team={status:true}">新增团队</button>
        </div>
        <br />
        <div class="card mx-4">
            <div class="-my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
                <div class="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
                    <div class="datatable">
                        <table>
                            <thead>
                                <tr>
                                    <th>团队名称</th>
                                    <th>团队标识</th>
                                    <th>状态</th>
                                    <th>创建时间</th>
                                    <th>操作</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="team of teams" :key="team.id" @click="selectRow(team,$event)">
                                    <td>{{ team.name }}</td>
                                    <td>{{ team.teamTags }}</td>
                                    <td>{{ team.status?"启用":"禁用" }}</td>
                                    <td>{{ $moment(team.createTime).format('lll') }}</td>
                                    <td>
                                        <button type="button" title="编辑" class="inline-flex btn btn-warning">
                                            <i class="material-icons">edit</i>
                                        </button>
                                        <button type="button" title="删除" class="inline-flex btn btn-danger" @click="deleteItem(team.id)">
                                            <i class="material-icons">delete</i>
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
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
                                            <option v-for="(item , index) in pageSizes" :key="index" :value="item">{{ item }}</option>
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
                    </div>
                </div>
            </div>
        </div>
        <TransitionRoot appear :show="modalOpen" as="template">
            <Dialog as="div" class="fixed z-10 inset-0 overflow-y-auto" @close="modalOpen = false">
                <div class="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
                    <TransitionChild as="template" enter="ease-out duration-300" enter-from="opacity-0" enter-to="opacity-100" leave="ease-in duration-200" leave-from="opacity-100" leave-to="opacity-0">
                        <DialogOverlay class="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity" />
                    </TransitionChild>

                    <!-- This element is to trick the browser into centering the modal contents. -->
                    <span class="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">&#8203;</span>
                    <TransitionChild as="template" enter="ease-out duration-300" enter-from="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95" enter-to="opacity-100 translate-y-0 sm:scale-100" leave="ease-in duration-200"
                                     leave-from="opacity-100 translate-y-0 sm:scale-100" leave-to="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95">
                        <form @submit.prevent="save" class="inline-block w-full align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg">
                            <DialogTitle as="h3" class="text-lg leading-6 font-medium text-gray-900 bg-gray-100 p-3">
                                新增团队
                            </DialogTitle>
                            <div class="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4">
                                <div class="mt-3 sm:mt-0 sm:ml-2">
                                    <div class="grid grid-cols-6 gap-6">
                                        <div class="col-span-6">
                                            <label for="name" class="block text-sm font-medium text-gray-700">团队名称</label>
                                            <input id="name" type="text" placeholder="团队名称" v-model="team.name" autocomplete="name"
                                                   class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md" />
                                        </div>
                                        <div class="col-span-6">
                                            <label for="teamTags" class="block text-sm font-medium text-gray-700">团队标识</label>
                                            <input id="teamTags" type="number" placeholder="团队标识" v-model="team.teamTags" autocomplete="price"
                                                   class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md" />
                                        </div>
                                        <div class="col-span-6">
                                            <input id="status" type="checkbox" placeholder="启用" v-model="team.status" autocomplete="category"
                                                   class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 shadow-sm sm:text-sm border-gray-300 rounded-md" />
                                            <label for="status" class="inline-flex text-sm font-medium text-gray-700 ml-2">启用</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="flex flex-row-reverse bg-gray-50 px-2 py-3 sm:px-6">
                                <button type="submit" class="inline-flex ml-3 btn btn-warning">
                                    保存
                                </button>
                                <button type="button" class="btn btn-default" @click="modalOpen = false" ref="cancelButtonRef">
                                    取消
                                </button>
                            </div>
                        </form>
                    </TransitionChild>
                </div>
            </Dialog>
        </TransitionRoot>
    </div>
</template>
<script setup>
    import Team from "../services/teams";
    import { onBeforeUpdate, onMounted, ref, computed } from "vue";
    import { usePagination } from 'vue-composable';
    import { ChevronLeftIcon, ChevronRightIcon } from "@heroicons/vue/solid";
    import { Dialog, DialogOverlay, DialogTitle, TransitionChild, TransitionRoot, } from "@headlessui/vue";
    const modalOpen = ref(false);
    const team = ref({
        name: "",
        teamTags: [],
        status: true,
    });
    const teams = ref([]);
    const list = () => {
        Team.list().then((response) => {
            teams.value = response.data;
        });
    };
    const save = () => {
        Team.save(team.value).then((response) => {
            if (team.value.id == null) teams.value.push(response.data);
            modalOpen.value = false;
        });
    };
    const deleteItem = (id) => {
        Team.delete(id).then(() => {
            list();
        });
    };
    const selectRow = (row, event) => {
        if (event.target.tagName == "TD") {
            team.value = row;
            modalOpen.value = true;
        }
    };
    onMounted(() => {
        list();
    });
    onBeforeUpdate(() => { });
    ////////////////////
    const pageSizes = [10, 20, 50, 100, 500];
    const { pageSize, total, currentPage, offset, lastPage, next, prev, first, last } = usePagination({
        currentPage: 1,
        pageSize: 20,
        total: computed(() => { return 100 * 10 })
    });
    const pageRender = computed(() => {
        let arr = Array.from(Array(lastPage.value - 2), (v, k) => k + 2);
        let result = currentPage.value < 5 ? arr.slice(0, 4) : (currentPage.value > lastPage.value - 4 ? arr.slice(-4) : arr.slice(currentPage.value - 3, currentPage.value));
        return result;
    });
</script>
<style>
    .datatable table {
        @apply min-w-full divide-y divide-gray-200;
    }

    .datatable thead {
        @apply bg-gray-50;
    }

    .datatable th {
        @apply px-6 py-3 text-sm font-medium tracking-wider text-gray-500 text-left uppercase;
    }

    .datatable td {
        @apply px-6 py-4 whitespace-nowrap text-sm;
    }

    .datatable tr:hover {
        @apply bg-gray-50;
    }
</style>
