import { Tasks } from '@/models/tasks-model';
import baseService from '@services/core/base-service';
import API_END_POINTS from '@utils/constants/api-end-points';

export const projectService = baseService.injectEndpoints({
  endpoints: (builder) => ({
    projects: builder.query<Tasks[], string>({
      query: (queryParams) => ({
        url: API_END_POINTS.projects + `?${queryParams}`,
        method: 'GET'
      }),
      providesTags: ['projects']
    }),
    project: builder.query<Tasks, number>({
      query: (projectId) => ({
        url: API_END_POINTS.projects + `/${projectId}`,
        method: 'GET'
      }),
      providesTags: ['project']
    }),
    projectSaved: builder.mutation<Tasks, Tasks>({
      query: (project) => {
        const requestUrl = project?.projectId ? API_END_POINTS.projects + `/${project.projectId}` : API_END_POINTS.projects;
        const requestMethod = project?.projectId ? 'PUT' : 'POST';
        
        return {
          url: requestUrl,
          method: requestMethod,
          body: project
        };
      },
      invalidatesTags: ['projects', 'projects']
    })
  }),
});

export const { useLazyProjectsQuery, useProjectSavedMutation, useProjectQuery } = projectService;