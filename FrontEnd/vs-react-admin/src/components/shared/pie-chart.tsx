import React from 'react';
import { PieChart, Pie, Cell, Tooltip, Legend, ResponsiveContainer } from 'recharts';

// Example data
const data = [
  { name: 'Free Employee', value: 100 },
  { name: 'Assigned Employee', value: 200 },
];

const COLORS = ['#00C49F', '#FF8042']; // Color for each pie section

const PieChartComponent: React.FC = () => {
  return (
    <ResponsiveContainer width="100%" height={400}>
      <PieChart>
        <Pie
          data={data}
          dataKey="value"
          nameKey="name"
          outerRadius={120}
          innerRadius={60}
          paddingAngle={5}
        >
          {data.map((entry, index) => (
            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
          ))}
        </Pie>
        <Tooltip />
        <Legend />
      </PieChart>
    </ResponsiveContainer>
  );
};

export default PieChartComponent;