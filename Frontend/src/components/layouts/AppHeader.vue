<template>
  <div class="header-inner">
    <el-button text @click="$emit('update:collapsed', !collapsed)">
      <el-icon><Menu /></el-icon>
    </el-button>

    <div class="right">
      <el-switch
        v-model="dark"
        active-text="Dark"
        inactive-text="Light"
        @change="toggleTheme"
      />

      <el-dropdown>
        <span class="user">
          Admin <el-icon><ArrowDown /></el-icon>
        </span>
        <template #dropdown>
          <el-dropdown-menu>
            <el-dropdown-item>Profile</el-dropdown-item>
            <el-dropdown-item divided>Logout</el-dropdown-item>
          </el-dropdown-menu>
        </template>
      </el-dropdown>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { Menu, ArrowDown } from '@element-plus/icons-vue'

defineProps<{ collapsed: boolean }>()
defineEmits(['update:collapsed'])

const dark = ref(false)

const toggleTheme = () => {
  document.documentElement.classList.toggle('dark', dark.value)
}
</script>

<style scoped>
.header-inner {
  width: 100%;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.right {
  display: flex;
  align-items: center;
  gap: 16px;
}

.user {
  cursor: pointer;
}
</style>
