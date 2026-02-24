<template>
	<div class="layout-container layout-padding">
		<el-card shadow="hover" class="layout-padding-auto">
			<div class="flex justify-between mb-4">
				<div class="flex items-center">
					<el-input
						size="default"
						v-model="state.tableData.page.search"
						placeholder="Search title..."
						style="max-width: 220px"
						class="mr-3"
					>
					</el-input>
					<el-button @click="getTableData" size="default" type="primary">
						<div class="flex items-center gap-2">
							<el-icon>
								<Search />
							</el-icon>
							Search
						</div>
					</el-button>
				</div>
				<el-button type="success" @click="openDialogRef?.openDialog('add')">
					<div class="flex items-center gap-2">
						<el-icon>
							<Plus />
						</el-icon>
						New
					</div>
				</el-button>
			</div>
			<el-table
				ref="tableRef"
				:data="state.tableData.data"
				v-loading.lock="state.tableData.config.loading"
				style="width: 100%"
				row-key="id"
				:default-sort="{ prop: 'createdAt', order: 'descending' }"
			>
				<el-table-column type="index" label="No" width="100px" />
				<el-table-column
					v-for="(item, index) in state.tableData.header"
					:key="index"
					:label="item.title"
					:prop="item.key"
					:width="item.width"
					:show-overflow-tooltip="true"
					:resizable="true"
					sortable
				>
					<template #default="scope">
						<el-tag
							v-if="item.type === 'tag'"
							:type="scope.row.is_visible ? 'success' : 'danger'"
						>
							{{ scope.row[item.key] ? "yes" : "no" }}
						</el-tag>
						<span v-else>
							{{ scope.row[item.key] }}
						</span>
					</template>
				</el-table-column>

				<el-table-column label="Operate" show-overflow-tooltip width="220">
					<template #default="scope">
						<el-button
							size="small"
							text
							type="primary"
							@click="onSystem(scope.row, 'view')"
						>
							View</el-button
						>
						<el-button
							size="small"
							text
							type="warning"
							@click="onSystem(scope.row, 'edit')"
						>
							Edit</el-button
						>
						<el-button
							size="small"
							text
							type="danger"
							@click="onSystem(scope.row, 'delete')"
							>Delete</el-button
						>
					</template>
				</el-table-column>
			</el-table>
			<el-pagination
				class="mt-4"
				v-model:current-page="state.tableData.page.page"
				v-model:page-size="state.tableData.page.pageSize"
				:page-sizes="[10, 25, 50, 75, 100]"
				:small="true"
				background
				layout="total, sizes, prev, pager, next, jumper"
				@size-change="onHandleSizeChange"
				@current-change="onHandleCurrentChange"
				:total="state.tableData.total"
			/>
		</el-card>
		<Dialog
			ref="openDialogRef"
			:data="state.tableData.data"
			@refresh="getTableData()"
		/>
		<View ref="openViewRef" />
	</div>
</template>
<script lang="ts" setup>
import { deleteNote, getNote, getNotes } from "@/services/note";
import { useStore } from "@/stores";
import dayjs from "dayjs";
import { ElMessageBox } from "element-plus";
import { defineAsyncComponent, onMounted, reactive, ref } from "vue";

const View = defineAsyncComponent(() => import("./view.vue"));
const Dialog = defineAsyncComponent(() => import("./dialog.vue"));
const store = useStore();
const state = reactive({
	// Header content (required, pay attention to the format)
	tableData: {
		header: [
			{
				key: "title",
				title: "Title",

				type: "text",
				isCheck: true,
			},

			{
				title: "Created At",
				key: "createdAt",
				width: 180,
				type: "date",
				isCheck: true,
			},
			{
				title: "Updated At",
				key: "updatedAt",
				width: 180,
				type: "date",
				isCheck: true,
			},
		],
		data: [] as EmptyObjectType[],
		page: {
			page: 1,
			pageSize: 30,
			search: "",
		},
		total: 0,
		config: {
			isBorder: false,
			isOperate: [
				{
					label: "Edit",
					key: "edit",
				},
				{
					label: "Delete",
					key: "delete",
					type: "tip",
					tipTitle: "Are you sure?",
				},
			],
			loading: false,
		},
	},
});
const openDialogRef = ref();
const getTableData = async () => {
	state.tableData.config.loading = true;
	try {
		const response = await getNotes(store.userInfo.id, state.tableData.page);
		const data = response.data ?? [];
		state.tableData.data = data.map((item: EmptyObjectType) => {
			return {
				...item,
				createdAt: dayjs(item.createdAt).format("DD/MM/YYYY HH:mm A"),
				updatedAt: dayjs(item.updatedAt).format("DD/MM/YYYY HH:mm A"),
			};
		});
		state.tableData.total = response.total;
	} catch (e) {
		console.log(e);
	} finally {
		state.tableData.config.loading = false;
	}
};
const openViewRef = ref();
const onSystem = async (row: Record<string, number>, key: string) => {
	if (key === "view") {
		const res = await getNote(store.userInfo.id, Number(row?.id));
		openViewRef.value?.openDialog(res);
	} else if (key === "edit") {
		openDialogRef.value?.openDialog(key, row);
	} else if (key === "delete") {
		onRowDel(row);
	}
};
// Pagination changes
const onHandleSizeChange = (val: number) => {
	state.tableData.page.pageSize = val;
	getTableData();
};
// Pagination changes
const onHandleCurrentChange = (val: number) => {
	state.tableData.page.page = val;
	getTableData();
};
const onRowDel = (row: EmptyObjectType) => {
	ElMessageBox.confirm(`This action will permanently delete?`, "Hint", {
		confirmButtonText: "yes",
		cancelButtonText: "no",
		type: "warning",
	})
		.then(async () => {
			await deleteNote(store.userInfo.id, row.id);
			getTableData();
		})
		.catch(() => {});
};
onMounted(() => getTableData());
</script>
