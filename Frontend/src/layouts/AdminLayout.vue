<template>
	<el-container class="layout">
		<!-- Sidebar -->
		<el-aside :width="collapsed ? '64px' : '220px'" class="sidebar">
			<AppSidebar :collapsed="collapsed" />
		</el-aside>

		<el-container>
			<!-- Header -->
			<el-header class="header">
				<AppHeader v-model:collapsed="collapsed" />
			</el-header>

			<!-- Main -->
			<el-main class="main">
				<el-breadcrumb separator="/" class="mb-5">
					<el-breadcrumb-item>Home</el-breadcrumb-item>
					<el-breadcrumb-item>{{ route.meta.title }}</el-breadcrumb-item>
				</el-breadcrumb>

				<router-view />
			</el-main>

			<!-- Footer -->
			<el-footer class="footer">
				<AppFooter />
			</el-footer>
		</el-container>
	</el-container>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useRoute } from "vue-router";

import AppFooter from "@/components/layouts/AppFooter.vue";
import AppHeader from "@/components/layouts/AppHeader.vue";
import AppSidebar from "@/components/layouts/AppSidebar.vue";

const route = useRoute();
const collapsed = ref(false);
</script>

<style scoped>
.layout {
	height: 100vh;
}

.sidebar {
	background: #001529;
	transition: width 0.2s;
}

.header {
	background: #fff;
	border-bottom: 1px solid #ebeef5;
	display: flex;
	align-items: center;
	padding: 0 16px;
}

.main {
	background: #f5f7fa;
	/* padding: 20px; */
}

.footer {
	height: 32px;
	text-align: center;
	margin-top: 12px;
	font-size: 12px;
	color: #999;
}
</style>
