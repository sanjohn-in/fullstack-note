<template>
	<div class="system-user-dialog-container">
		<el-dialog
			v-model="state.dialog.isShowDialog"
			:title="state.dialog.title"
			destroy-on-close
			@close="resetFields"
			width="550px"
		>
			<template #title>{{ state.dialog.title }}</template>
			<template #default>
				<el-form
					class="p-5"
					ref="formRef"
					:model="state.form"
					:rules="rules"
					label-position="left"
					label-width="120px"
				>
					<el-row :gutter="20">
						<!-- Title -->
						<el-col :xs="24" :md="24">
							<el-form-item prop="title" label="title">
								<el-input v-model="state.form.title" type="text" />
							</el-form-item>
						</el-col>

						<!-- Content -->
						<el-col :xs="24" :md="24">
							<el-form-item prop="content" label="content">
								<el-input v-model="state.form.content" type="textarea" />
							</el-form-item>
						</el-col>
					</el-row>
				</el-form>
			</template>

			<template #footer>
				<div class="dialog-footer">
					<el-button type="primary" :loading="state.loading" @click="submitProcess">
						<div class="flex items-center gap-2">
							<el-icon>
								<Check />
							</el-icon>
							Save
						</div>
					</el-button>
				</div>
			</template>
		</el-dialog>
	</div>
</template>
<script setup lang="ts">
import { createNote, updateNote } from "@/services/note";
import { Check } from "@element-plus/icons-vue";
import type { FormInstance, FormRules } from "element-plus";
import { ElMessage } from "element-plus";
import { reactive, ref } from "vue";

const state = reactive({
	form: {
		id: null,
		title: "",
		content: "",
	},
	dialog: {
		title: "",
		submit: "",
		cancel: "",
		isShowDialog: false,
		type: "",
	},
	loading: false,
});
const formRef = ref<FormInstance>();

const rules: FormRules = {
	title: [
		{ required: true, message: "Title is required", trigger: "blur" },
		{ min: 3, message: "At least 3 characters", trigger: "blur" },
	],
	content: [
		{ required: true, message: "Content is required", trigger: "blur" },
		{ min: 10, message: "At least 10 characters", trigger: "blur" },
	],
};

const emit = defineEmits(["refresh"]);
const resetFields = () => {
	state.form = {
		id: null,
		title: "",
		content: "",
	};
};

const openDialog = async (type: string, row: object) => {
	state.dialog.type = type;
	state.dialog.title = type === "add" ? "Add Note" : "Edit Note";
	state.dialog.isShowDialog = true;
	state.form = JSON.parse(JSON.stringify(row));
};
const submitProcess = async () => {
	await formRef.value?.validate(async (valid) => {
		if (!valid) return;
		try {
			state.loading = true;
			const request = {
				id: state.form.id,
				title: state.form.title,
				content: state.form.content,
			};
			const response = state.form.id
				? await updateNote(state.form.id, request)
				: await createNote(request);
			state.dialog.isShowDialog = false;
			resetFields();
			ElMessage.success("Note saved successfully");
			console.log(response);
			emit("refresh");
		} catch (e) {
			ElMessage.error("Failed to save note");
			console.log(e);
			//TODO handle the exception
		} finally {
			state.loading = false;
		}
	});
};
defineExpose({
	openDialog,
});
</script>
