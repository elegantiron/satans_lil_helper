using System;
using Microsoft.Xna.Framework;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.Utils;

internal static class ShadowCast
{
    private class OctantTransform
    {
        public int xx { get; private set; }
        public int xy { get; private set; }
        public int yx { get; private set; }
        public int yy { get; private set; }

        public OctantTransform(int xx, int xy, int yx, int yy)
        {
            this.xx = xx;
            this.xy = xy;
            this.yx = yx;
            this.yy = yy;
        }

        public override string ToString()
        {
            return string.Format(
                "[OctantTransform {0,2:D} {1,2:D} {2,2:D} {3,2:D}]",
                xx,
                xy,
                yx,
                yy
            );
        }

        private static OctantTransform[] s_octantTransform =
        {
            new OctantTransform(1, 0, 0, 1), // 0 E-NE
            new OctantTransform(0, 1, 1, 0), // 1 NE-N
            new OctantTransform(0, -1, 1, 0), // 2 N-NW
            new OctantTransform(-1, 0, 0, 1), // 3 NW-W
            new OctantTransform(-1, 0, 0, -1), // 4 W-SW
            new OctantTransform(0, -1, -1, 0), // 5 SW-S
            new OctantTransform(0, 1, -1, 0), // 6 S-SE
            new OctantTransform(1, 0, 0, -1), // 7 SE-E
        };

        public static void ComputeVisibility(ICellGrid grid, Point gridPosn, float viewRadius)
        {
            grid.SetLight(gridPosn, 0.0f);

            for (int txidx = 0; txidx < s_octantTransform.Length; txidx++)
            {
                CastLight(grid, gridPosn, viewRadius, 1, 1.0f, 0.0f, s_octantTransform[txidx]);
            }
        }
    }

    private static void CastLight(
        ICellGrid grid,
        Point gridPosn,
        float viewRadius,
        int startColumn,
        float leftViewSlope,
        float rightViewSlope,
        OctantTransform txfrm
    )
    {
        float viewRadiusSq = viewRadius * viewRadius;

        int viewCeiling = (int)Math.Ceiling(viewRadius);

        bool prevWasBlocked = false;

        float savedRightSlope = -1;

        int xDim = grid.XDim;
        int yDim = grid.YDim;

        for (int currentCol = startColumn; currentCol <= viewCeiling; currentCol++)
        {
            int xc = currentCol;

            for (int yc = currentCol; yc >= 0; yc--)
            {
                int gridX = gridPosn.X + xc * txfrm.xx + yc * txfrm.xy;
                int gridY = gridPosn.Y + xc * txfrm.yx + yc * txfrm.yy;

                if (gridX < 0 || gridX >= xDim || gridY < 0 || gridY >= yDim)
                {
                    continue;
                }

                float leftBlockSlope = (yc + 0.5f) / (xc - 0.5f);
                float rightBlockSlope = (yc - 0.5f) / (xc + 0.5f);

                if (rightBlockSlope > leftViewSlope)
                {
                    continue;
                }
                else if (leftBlockSlope < rightViewSlope)
                {
                    break;
                }

                float distanceSquared = xc * xc + yc * yc;
                if (distanceSquared <= viewRadiusSq)
                {
                    grid.SetLight(new Point(gridX, gridY), distanceSquared);
                }

                bool curBlocked = !grid.PassesLight(new Point(gridX, gridY));

                if (prevWasBlocked)
                {
                    if (curBlocked)
                    {
                        savedRightSlope = rightBlockSlope;
                    }
                    else
                    {
                        prevWasBlocked = false;
                        leftViewSlope = savedRightSlope;
                    }
                }
                else
                {
                    if (curBlocked)
                    {
                        if (leftBlockSlope <= leftViewSlope)
                        {
                            CastLight(
                                grid,
                                gridPosn,
                                viewRadius,
                                currentCol + 1,
                                leftViewSlope,
                                leftBlockSlope,
                                txfrm
                            );
                        }

                        prevWasBlocked = true;
                        savedRightSlope = rightBlockSlope;
                    }
                }
            }

            if (prevWasBlocked)
            {
                break;
            }
        }
    }
}
