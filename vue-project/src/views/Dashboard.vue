<template>
    <main>
        <h1>Client</h1>
        <div class="chat-container">
            <div class="chat-messages">
                <div>Messages:</div>
                <div class="message" v-for="message in messages" :key="message.messageId" style="margin-bottom: 2rem;">
                    <div>id: {{ message.messageId }}</div>
                    <div>content: {{ message.content }}</div>
                    <div>sent at: {{ message.sentAt }}</div>
                </div>
            </div>
            <form @submit.prevent="sendMessage()">
                <div>
                    <label for="">Start</label>
                    <Calendar id="calendar-24h" v-model="dateTimeStart" showTime hourFormat="24" />
                </div>
                <div>
                    <label for="">End</label>
                    <Calendar id="calendar-24h" v-model="dateTimeEnd" showTime hourFormat="24" />
                </div>
                <button type="submit">Send</button>
            </form>
        </div>
    </main>
</template>
  
<script setup>
import api from '@/api.json';
import { ref, watch } from "vue";
import { useWebSocket } from '@vueuse/core';
import axios from 'axios';
import Calendar from 'primevue/calendar';

const dateTimeStart = ref();
const dateTimeEnd = ref();
const messages = ref();

const sendMessage = () => {
    axios({
        method: 'get',
        url: `${api.http}/message`,
        params: {
            dateTimeStart: dateTimeStart.value.toJSON(),
            dateTimeEnd: dateTimeEnd.value.toJSON()
        }
    })
    .then(function (response) {
        messages.value = response.data;
    });
}

</script>
  