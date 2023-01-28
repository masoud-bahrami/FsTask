import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import { PieChart, Pie , Tooltip} from "recharts";

import './custom.css';
const data = [
  {name:"Facebook" , value:20000000},
  {name:"Instagram" , value:15000000},
  {name:"Twitter" , value:40000000},
  {name:"Telegram" , value:5000000},
];

export default class App extends Component {
  static displayName = App.name;


    render() {
        return (
                <PieChart width={400} height={400}>
                    <Pie
                        dataKey="value"
                        isAnimationActive={false}
                        data={data}
                        cx="50%"
                        cy="50%"
                        outerRadius={80}
                        fill="#8884d8"
                        label
                    />
                    <Tooltip />
                </PieChart>
        );
    }

  //render() {
  //  return (
  //    <Layout>
  //      <Routes>
  //        {AppRoutes.map((route, index) => {
  //          const { element, ...rest } = route;
  //          return <Route key={index} {...rest} element={element} />;
  //        })}
  //      </Routes>
  //    </Layout>
  //  );
  //}
}
