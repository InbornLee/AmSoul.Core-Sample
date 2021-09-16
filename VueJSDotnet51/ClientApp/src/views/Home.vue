<template>
  <div class="bg-gray-100">
    <div class="card mx-4">
      <form @submit.prevent="save">
        <div class="card-header">
          <h3 class="text-lg leading-6 font-medium text-gray-900">
            首页
          </h3>
          <span class="mt-1 max-w-2xl text-sm text-gray-500">
            Home.
          </span>
        </div>
        <div class="card-body">
          <div class="grid grid-cols-6 gap-6">
            <div class="col-span-6">
              <label for="name" class="block text-sm font-medium text-gray-700">名称</label>
              <input id="name" type="text" placeholder="名称" v-model="book.name" autocomplete="name"
                     class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md" />
            </div>
            <div class="col-span-6 sm:col-span-3 lg:col-span-2">
              <label for="price" class="block text-sm font-medium text-gray-700">价格</label>
              <input id="price" type="number" placeholder="价格" v-model="book.price" autocomplete="price"
                     class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md" />
            </div>
            <div class="col-span-6 sm:col-span-3 lg:col-span-2">
              <label for="category" class="block text-sm font-medium text-gray-700">类目</label>
              <input id="category" type="text" placeholder="类目" v-model="book.category" autocomplete="category"
                     class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md" />
            </div>
            <div class="col-span-6 sm:col-span-3 lg:col-span-2">
              <label for="author" class="block text-sm font-medium text-gray-700">作者</label>
              <input id="author" type="text" placeholder="作者" v-model="book.author" autocomplete="category"
                     class="mt-1 focus:ring-indigo-500 focus:border-indigo-500 block w-full shadow-sm sm:text-sm border-gray-300 rounded-md" />
            </div>
          </div>
        </div>
        <div class="card-footer">
          <button type="submit" class="inline-flex btn btn-primary">
            保存
          </button>
        </div>
      </form>
    </div>
    <br />
    <div class="card mx-4">
      <div class="-my-2 overflow-x-auto sm:-mx-6 lg:-mx-8">
        <div class="inline-block min-w-full py-2 align-middle sm:px-6 lg:px-8">
          <div class="overflow-hidden border-b border-gray-200 shadow sm:rounded-lg">
            <table class="datatable">
              <thead>
                <tr>
                  <th scope="col">名称</th>
                  <th scope="col">价格</th>
                  <th scope="col">类目</th>
                  <th scope="col">作者</th>
                  <th scope="col"><span class="sr-only">Edit</span></th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="book of books" :key="book.id">
                  <td>{{ book.name }}</td>
                  <td>{{ book.price }}</td>
                  <td>{{ book.category }}</td>
                  <td>{{ book.author }}</td>
                  <td class="px-6 py-4 text-sm font-medium text-right whitespace-nowrap">
                    <button type="button" title="编辑" class="inline-flex btn btn-warning">
                      <i class="material-icons">edit</i>
                    </button>
                    <button type="button" title="删除" class="inline-flex btn btn-danger" @click="deleteItem(book.id)">
                      <i class="material-icons">delete</i>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup>
import Book from "../services/books";
import { onMounted, ref } from "vue";
const book = ref({
  name: "",
  price: "",
  category: "",
  author: "",
});
const books = ref([]);
const list = () => {
  Book.list().then((response) => {
    //alert(JSON.stringify(response.data));
    books.value = response.data;
  });
};
const save = () => {
  Book.save(book.value).then((response) => {
    book.value = {};
    books.value.push(response.data);
  });
};
const deleteItem = (id) => {
  //alert(id);
  Book.delete(id).then(() => {
    //alert(JSON.stringify(response));
    list();
  });
};
onMounted(() => {
  list();
});
</script>
