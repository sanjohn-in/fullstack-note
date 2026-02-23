<template>
	<el-menu
		:collapse="collapsed"
		router
		background-color="#001529"
		text-color="#fff"
		active-text-color="#409EFF"
		class="menu"
	>
		<el-menu-item index="/dashboard">
			<el-icon><Monitor /></el-icon>
			<span>Dashboard</span>
		</el-menu-item>

		<el-menu-item index="/users">
			<el-icon><User /></el-icon>
			<span>Users</span>
		</el-menu-item>

		<el-menu-item index="/settings">
			<el-icon><Setting /></el-icon>
			<span>Settings</span>
		</el-menu-item>
	</el-menu>
</template>

<script setup lang="ts">
import { Monitor, Setting, User } from "@element-plus/icons-vue";

defineProps<{
	collapsed: boolean;
}>();
</script>

<style scoped>
.menu {
	height: 100%;
	border-right: none;
}
</style>
