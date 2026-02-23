<template>
	<div class="system-user-dialog-container">
		<el-dialog
			v-model="state.dialog.isShowDialog"
			title="Note Details"
			width="550px"
		>
			<template #default>
				<el-descriptions v-if="state.note" :column="1" border>
					<el-descriptions-item label="ID">
						{{ state.note.id }}
					</el-descriptions-item>
					<el-descriptions-item label="Title">
						{{ state.note.title }}
					</el-descriptions-item>
					<el-descriptions-item label="Content">
						<div style="white-space: pre-wrap">{{ state.note.content }}</div>
					</el-descriptions-item>
					<el-descriptions-item label="Created At">
						{{ state.note.createdAt }}
					</el-descriptions-item>
					<el-descriptions-item label="Updated At">
						{{ state.note.updatedAt }}
					</el-descriptions-item>
				</el-descriptions>
			</template>
			<template #footer>
				<div class="dialog-footer">
					<el-button @click="state.dialog.isShowDialog = false"
						>Close</el-button
					>
				</div>
			</template>
		</el-dialog>
	</div>
</template>

<script setup lang="ts">
import { reactive } from "vue";

const state = reactive({
	dialog: { isShowDialog: false },
	note: null as any,
});

const openDialog = (row: object) => {
	state.note = row;
	state.dialog.isShowDialog = true;
};

defineExpose({ openDialog });
</script>
