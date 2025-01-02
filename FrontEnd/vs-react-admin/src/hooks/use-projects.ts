import { useEffect, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { App } from 'antd';
import useFilter from '@hooks/utility-hooks/use-filter';
import { Tasks } from '@models/tasks-model';
import { AppError, QueryParams } from '@models/utils-model';
import { useLazyProjectsQuery, useProjectSavedMutation, useProjectQuery } from '@services/project-service';
import { formatQueryParams } from '@utils/helpers';

export const useProjects = () =>{
  const location = useLocation();

  const { getQueryParams, setQueryParams, getDefaultQueryParams } = useFilter();
  const queryParams = getQueryParams();

  const [filterParams, setFilterParams] = useState<QueryParams>({});
  
  const [onFetching, { isFetching, data: response }] = useLazyProjectsQuery();

  useEffect(() => {
    setFilterParams(queryParams);
  }, [location.search]);
  
  useEffect(() => {
    const newQueryParams = {
      ...filterParams,
      ...getDefaultQueryParams()
    };
    
    setQueryParams(newQueryParams);
    setFilterParams(newQueryParams);
    
    onFetching(formatQueryParams(filterParams));
  }, []);

  const loading = isFetching;
  
  return {
    isLoading: loading,
    data: response
  };
};

export const useProjectForm = () => {
  const { message } = App.useApp();
  const navigate = useNavigate();
  
  const [projectSaved, { isLoading, isSuccess, isError, error }] = useProjectSavedMutation();
  
  useEffect(() => {
    if (isSuccess) {
      message.success('project saved successfully.');
      navigate('/projects');
    }
    
    if (isError && error) {
      message.error((error as AppError).data.error_description);
    }
  }, [isSuccess, isError, error]);
  
  const onSaved = (Tasks: Tasks) => {
    projectSaved({
      ...Tasks
    });
  };
  
  return {
    isLoading,
    onSaved
  };
};

export const useproject = (projectId: number) => {
  const { isLoading, data: project } = useProjectQuery(projectId);
  
  return {
    isLoading,
    project
  };
};