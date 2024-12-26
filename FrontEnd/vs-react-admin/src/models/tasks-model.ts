export interface Tasks {
    id?: number;
    title: string;
    description: string;
    status: string;
    start_date: string;
    end_date: string;
}

export type TasksPartial = Partial<Tasks>;

export interface Task {
    task: Tasks[];
}