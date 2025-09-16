import { Chart, useChart } from "@chakra-ui/charts"
import { Cell, Label, Pie, PieChart } from "recharts"

export default function RatingChart({ percent }: { percent: number }) {
  const chart = useChart({
    data: [
      { name: "no", value: 100 - percent, color: "black.solid" },
      { name: "yes", value: percent, color: "green.solid" },
    ],
  })

  return (
    <Chart.Root boxSize="50px" mt="8" ml="5" chart={chart}  >
      <PieChart>
        <Pie
          innerRadius={40}
          outerRadius={45}
          isAnimationActive={false}
          data={chart.data}
          dataKey={chart.key("value")}
          nameKey="name"
        >
          <Label
            content={({ viewBox }) => (
              <Chart.RadialText fontSize="1.8rem"
                viewBox={viewBox}
                title={percent.toFixed(0) + "%"}
                description=""
              />
            )}
          />
          {chart.data.map((item) => (
            <Cell key={item.color} fill={chart.color(item.color)} />
          ))}
        </Pie>
      </PieChart>
    </Chart.Root>
  )
}