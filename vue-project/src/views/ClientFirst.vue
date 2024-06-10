<template>
  <main>
    <h1>Client</h1>
    <div class="chat-container">
      <div class="chat-messages">
        <div class="message" v-for="message in messages" :key="message.MessageIndex" style="margin-bottom: 2rem;">
          <div>Index: {{ message.MessageIndex }}</div>
          <div>Content: {{ message.Content }}</div>
          <div>Time: {{ message.SentAt }}</div>
        </div>
      </div>
      <form @submit.prevent="sendMessage()">
        <input v-model="inputMessage" type="text">
        <button type="submit">Send</button>
      </form>
    </div>
  </main>
</template>

<script setup>
import api from '@/api.json';
import { ref, watch } from "vue";
import { useWebSocket } from '@vueuse/core';

const { data, send, open } = useWebSocket(api.ws);

open();

const inputMessage = ref();
const messages = ref([]);

const sendMessage = () => {
  send(inputMessage.value);
  inputMessage.value = '';
}

watch(data, (oldData, newData) => {
  let dataJson = JSON.parse(data.value);
  messages.value.push(dataJson);
  console.log(dataJson);
});
</script>
